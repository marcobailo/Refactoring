using System.IO;
using System.Text;
using System.Web;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{

    public interface IToHtmlTextConverter
    {

        string ConvertToHtml(string title = null);

        bool Validate();

    }

    public abstract class ToHtmlTextConverterBase : IToHtmlTextConverter
    {

        public abstract string ConvertToHtml(string title = null);

        public virtual bool Validate() { return false; }

        protected StringBuilder CheckHtmlFromText(StringBuilder text)
        {
            text = text.Replace("\r\n", "\r");
            text = text.Replace("\n", "\r");
            text = text.Replace("\r", "<br>\r\n");
            text = text.Replace("  ", " &nbsp;");
            return text;
        }

        protected StringBuilder DecorateConvertedPlanTextToHtml(StringBuilder text, string title = null)
        {
            if (text != null && text.Length > 0)
            {
                string _header = "<!DOCTYPE html><html><head>";
                if (string.IsNullOrEmpty(title) == false && title.Trim() != "")
                    _header += "<title>" + title + "</title>";
                _header += "</head><body>";
                text.Insert(0, _header);
                text.Append("</body></html>");
            }
            return text;
        }

    }

    public class UnicodeFileToHtmlTextConverter : ToHtmlTextConverterBase
    {

        private readonly string _fullFilenameWithPath;

        public UnicodeFileToHtmlTextConverter(string fullFilenameWithPath)
        {
            _fullFilenameWithPath = fullFilenameWithPath;
        }

        public override bool Validate()
        {
            bool _validated = false;
            if (!(string.IsNullOrEmpty(_fullFilenameWithPath) || _fullFilenameWithPath.Trim() == ""))
            {
                if (File.Exists(_fullFilenameWithPath))
                {
                    if (new FileInfo(_fullFilenameWithPath).Length > 0)
                        _validated = true;
                }
            }
            return _validated;
        }

        public override string ConvertToHtml(string title = null)
        {
            string _line;
            TextReader _unicodeFileStream = File.OpenText(_fullFilenameWithPath);
            StringBuilder _fileContent = new StringBuilder();
            while ((_line = _unicodeFileStream.ReadLine()) != null)
            {
                _fileContent.Append(HttpUtility.HtmlEncode(_line));
                _fileContent.Append("<br />");
            }
            _unicodeFileStream.Close();
            _unicodeFileStream.Dispose();
            return DecorateConvertedPlanTextToHtml(CheckHtmlFromText(_fileContent), title).ToString();
        }

    }

    public class ByteArrayToHtmlTextConverter : ToHtmlTextConverterBase
    {

        private readonly byte[] _planTextContentAsByteArray;

        public override bool Validate()
        {
            return !(_planTextContentAsByteArray == null || _planTextContentAsByteArray.Length == 0);
        }

        public ByteArrayToHtmlTextConverter(byte[] bytesArray)
        {
            _planTextContentAsByteArray = bytesArray;
        }

        public override string ConvertToHtml(string title = null)
        {
            return DecorateConvertedPlanTextToHtml(CheckHtmlFromText(new StringBuilder(Encoding.UTF8.GetString(_planTextContentAsByteArray))), title).ToString();
        }

    }

    public class StringToHtmlTextConverter : ToHtmlTextConverterBase
    {

        private readonly string _planText;

        public override bool Validate()
        {
            return !(string.IsNullOrEmpty(_planText) || _planText.Trim() == "");
        }

        public StringToHtmlTextConverter(string planText)
        {
            _planText = planText;
        }

        public override string ConvertToHtml(string title = null)
        {
            return DecorateConvertedPlanTextToHtml(CheckHtmlFromText(new StringBuilder(_planText)), title).ToString();
        }

    }

}


// old source code
/*
using System.IO;
using System.Web;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    public class UnicodeFileToHtmlTextConverter
    {
        private readonly string _fullFilenameWithPath;


        public UnicodeFileToHtmlTextConverter(string fullFilenameWithPath)
        {
            _fullFilenameWithPath = fullFilenameWithPath;
        }

        public string ConvertToHtml()
        {
            using (TextReader unicodeFileStream = File.OpenText(_fullFilenameWithPath))
            {
                string html = string.Empty;

                string line = unicodeFileStream.ReadLine();
                while (line != null)
                {
                    html += HttpUtility.HtmlEncode(line);
                    html += "<br />";
                    line = unicodeFileStream.ReadLine();
                }

                return html;
            }
        }
    }
}

*/