using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDefinitions;

/// <summary>
/// Support for trace to a text file
/// </summary>
/// <remarks>
/// Modified so the file is only opened if the TraceToFile menu option is selected
/// </remarks>
public class TraceToFileSupport
{
    [Flags]
    public enum TraceToFileState
    {
        NotInitialized = 0,
        Initialized = 1,
        StreamOpened = 2,
        StreamClosed = 4
    }
    public StreamWriter TraceStream
    {
        get; protected set;
    }
    public TextWriterTraceListener WriterTraceListener
    {
        get; protected set;
    }
    public string Filename
    {
        get;
        protected set;
    }
    public TraceToFileState State
    {
        get; protected set;
    }
    public void Initialize(string path)
    {
        Filename = path;
        State = TraceToFileState.Initialized;
    }
    protected void OpenStream()
    {
        // only open the stream if it has not been previously opened
        if (State == TraceToFileState.Initialized)
        {
            TraceStream = new StreamWriter(Filename, File.Exists(Filename));
            State = TraceToFileState.StreamOpened;
        }
        else if (State == TraceToFileState.NotInitialized && string.IsNullOrEmpty(Filename))
            throw new InvalidOperationException("TraceToFileSupport not initialized, no filename is defined");
    }
    public void StartListening()
    {
        OpenStream();

        Trace.Listeners.Add(WriterTraceListener ??= new TextWriterTraceListener(TraceStream));
    }
    public void StopListening()
    {
        if (WriterTraceListener != null)
        {
            Trace.Listeners.Remove(WriterTraceListener);
        }
    }
    public void FinishListening()
    {
        StopListening();

        if (State == TraceToFileState.StreamOpened)
        {
            TraceStream?.Flush();
            TraceStream?.Close();
        }
        WriterTraceListener?.Dispose();
        State = TraceToFileState.StreamClosed;
    }
}
