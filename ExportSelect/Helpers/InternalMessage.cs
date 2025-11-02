using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportSelect.Helpers;

public enum InternalMessageType
{
    GenerateCode,
    GenerateCodeResults,
    ResetInternalState,
    ClearInternalState
}
public class InternalMessageEventArgs : EventArgs
{
    public InternalMessageType MessageType;
    public string MessageContent;
    public InternalMessageEventArgs(InternalMessageType internalMessage,string messageContent)
    {
        MessageType = internalMessage;
        // MessageContent can be null or empty depending on the MessageType, GenerateCodeResults will have content, GenerateCode, ResetInternalState and ClearInternalState will not
        MessageContent = messageContent;
    }
}
