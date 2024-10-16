using BToolbox.GUI.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BToolbox.Logger
{
    public class LogRichTextBoxManager
    {

        public LogRichTextBoxManager(RichTextBox logTextBox)
            => _logTextBox = logTextBox;

        public void Init()
        {
            _logTextBox.Resize += LogTextBoxResize;
            LogDispatcher.NewLogMessage += NewLogMessageHandler;
            ReloadLogMessages();
        }

        private readonly RichTextBox _logTextBox;

        #region Display settings
        private bool _showVerboseLog;

        public bool ShowVerboseLog
        {
            get => _showVerboseLog;
            set
            {
                if (_showVerboseLog == value)
                    return;
                _showVerboseLog = value;
                ReloadLogMessages();
            }
        }

        public LogMessageSeverity MaxSeverityNormal { get; init; } = LogMessageSeverity.Info;
        public LogMessageSeverity MaxSeverityVerbose { get; init; } = LogMessageSeverity.Verbose;

        private readonly Dictionary<LogMessageSeverity, Color> _logColors = new()
        {
            { LogMessageSeverity.Error, Color.Red },
            { LogMessageSeverity.Warning, Color.Orange },
            { LogMessageSeverity.Info, Color.Black },
            { LogMessageSeverity.Verbose, Color.Blue },
            { LogMessageSeverity.VerbosePlus, Color.BlueViolet }
        };

        public void SetColor(LogMessageSeverity severity, Color color)
        {
            _logColors[severity] = color;
            ReloadLogMessages();
        }
        #endregion

        #region Message handler
        private void NewLogMessageHandler(DateTime Timestamp, LogMessageSeverity severity, string message)
           => _logTextBox.InvokeIfRequired(() => AddLogMessage(Timestamp, severity, message));

        private void AddLogMessage(DateTime timestamp, LogMessageSeverity severity, string message)
        {
            if ((severity > MaxSeverityVerbose) || (!_showVerboseLog && (severity > MaxSeverityNormal)))
                return;
            string textToAdd = $"[{timestamp:HH:mm:ss}] {message}\r\n";
            _logTextBox.AppendText(textToAdd);
            int textLength = _logTextBox.TextLength;
            int selectionLength = textToAdd.Length;
            int selectionStart = textLength - selectionLength + 1;
            if (selectionStart < 0)
            {
                selectionStart = 0;
                selectionLength = 0;
            }
            _logTextBox.Select(selectionStart, selectionLength);
            _logTextBox.SelectionColor = _logColors[severity];
            LogTextBoxScrollToEnd();
        }

        private void ReloadLogMessages()
        {
            _logTextBox.Text = string.Empty;
            List<LogMessage> messages = new(LogDispatcher.Messages);
            foreach (LogMessage logMessage in messages)
                AddLogMessage(logMessage.Timestamp, logMessage.Severity, logMessage.Message);
        }
        #endregion

        private void LogTextBoxResize(object sender, EventArgs e)
            => LogTextBoxScrollToEnd();

        private void LogTextBoxScrollToEnd()
        {
            _logTextBox.SelectionStart = _logTextBox.Text.Length;
            _logTextBox.ScrollToCaret();
        }

    }
}
