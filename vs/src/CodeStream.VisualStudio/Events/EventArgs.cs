﻿using System;

namespace CodeStream.VisualStudio.Events
{
    public class EventArgsBase : EventArgs
    {

    }

    public sealed class LanguageServerReadyEvent : EventArgsBase
    {
        public bool IsReady { get; set; }
    }

    public sealed class SessionReadyEvent : EventArgsBase { }

    public sealed class SessionLogoutEvent : EventArgsBase { }

    public enum TextDocumentChangedReason
    {
        Unknown,
        Scrolled,
        Edited,
        ViewportHeightChanged
    }

    public sealed class TextDocumentChangedEvent : EventArgsBase
    {
        public TextDocumentChangedReason Reason { get; set; }
    }

    public sealed class TextSelectionChangedEvent : EventArgsBase
    {
        
    }

    public sealed class CodemarkChangedEvent : EventArgsBase
    {
        public string Uri { get; set; }
    }

    public sealed class CodemarkVisibilityEvent : EventArgsBase
    {
        public bool IsVisible { get; set; }
    }

    public sealed class CodeStreamConfigurationChangedEvent : EventArgsBase
    {
         public bool OpenCommentOnSelect { get; set; }
    }
}
