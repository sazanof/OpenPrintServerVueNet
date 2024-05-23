﻿using OpenPrintServerVueNet.Exceptions;
using OpenPrintServerVueNet.Classes.Spool.Native;
using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;
using System.Threading.Channels;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace OpenPrintServerVueNet.Classes.Spool
{

    public partial class PrintWatcher : IDisposable {

        private IntPtr PrinterHandle;
        private IntPtr EventHandle;
        private Printer_Notify_Options2 Options;

        private PrintWatcher() {

        }

        private bool Disposed;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool Disposing) {
            lock (this) {
                if (Disposed) {
                    return;
                }

                Disposed = true;
            }

            if (Disposing) {
                Writer.Complete();
            }

            var OldHandle = PrinterHandle;
            PrinterHandle = default;

            Win32.ClosePrinter(OldHandle);
            Win32.FindClosePrinterChangeNotification(OldHandle);
            
        }


        ~PrintWatcher() {
            Dispose(false);
        }

        public ChannelReader<PrintWatcherEventArgs> Events { get; private set; }
        private ChannelWriter<PrintWatcherEventArgs> Writer { get; set; }

        private void Start() {

            var _mrEvent = new ManualResetEvent(false) {
                SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(EventHandle, false)
            };

            var _waitHandle = RegisterEvent(_mrEvent);
            
        }

        private RegisteredWaitHandle RegisterEvent(ManualResetEvent _mrEvent) {
            var _waitHandle = ThreadPool.RegisterWaitForSingleObject(_mrEvent, (_, TimedOut) => ThreadPoolCallback(TimedOut, _mrEvent), null, -1, true);
            return _waitHandle;
        }


        private void ThreadPoolCallback(bool TimedOut, ManualResetEvent _mrEvent) {
            if (!TimedOut) {
                if (Win32.FindNextPrinterChangeNotification(EventHandle, Options, out var Args)) {
                    
                    Writer.TryWrite(Args);

                    RegisterEvent(_mrEvent);
                }
                else {
                    this.Dispose();
                }
            }
        }

        

        public static PrintWatcher Start(PrintWatcherStartArgs StartInfo) {
            if(StartInfo == default) {
                throw new ArgumentNullException(nameof(StartInfo));
            }

            //Check to see if we asked for DevMode fields.
            //If we did, make sure that DevMode is a field we're retrieving
            var PrintDeviceFields = new List<PrintDeviceField>(StartInfo.PrintDeviceFields);
            
            var PrintJobFields = new List<PrintJobField>(StartInfo.PrintJobFields);
            

            //Determine our Event Filter
            var PrintDeviceEvent = StartInfo.PrintDeviceEvents.Aggregate(PrintDeviceEvents.None, (x, y) => x | y);

            //Open the Printer
            var Opened = Win32.OpenPrinter(StartInfo.PrintDeviceName, out var PrinterHandle);
            if (!Opened) {
                throw new UnableToOpenPrinterException(StartInfo.PrintDeviceName);
            }

            var FirstOptions = new Printer_Notify_Options2() {
                Children = {
                    NotifyOptions2.From(PrintDeviceFields),
                    NotifyOptions2.From(PrintJobFields),
                }
            };

            var EventHandle = Win32.FindFirstPrinterChangeNotification(PrinterHandle, PrintDeviceEvent, StartInfo.PrintDeviceHardwareType, FirstOptions);
            if(EventHandle == IntPtr.Zero || EventHandle == Win32.Invalid_Handle) {
                Win32.ClosePrinter(PrinterHandle);
                throw new UnableToMonitorPrinterEventsException(StartInfo.PrintDeviceName);
            }

            var SecondOptions = new Printer_Notify_Options2() {
                Flags = StartInfo.GetAllFieldsOnChange ? NotifyOptionsFlags.Refresh : NotifyOptionsFlags.None,
                Children = {
                    NotifyOptions2.From(PrintDeviceFields),
                    NotifyOptions2.From(PrintJobFields),
                }
            };

            var Channels = Channel.CreateUnbounded<PrintWatcherEventArgs>(new UnboundedChannelOptions() {
                SingleReader = false,
                SingleWriter = false,
            });

            var ret = new PrintWatcher() {
                PrinterHandle = PrinterHandle,
                EventHandle = EventHandle,
                Options = SecondOptions,
                Writer = Channels.Writer,
                Events = Channels.Reader,
            };
            ret.Start();

            return ret;
        }


    }

}
