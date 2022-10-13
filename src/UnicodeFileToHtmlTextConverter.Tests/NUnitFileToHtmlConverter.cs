using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter.Tests
{

    [TestFixture]
    class NUnitFileToHtmlConverter
    {

        private IToHtmlTextConverter _converterFromPlanTextToHtml = null;
        private string _path;
        private string _planFileName;

        [SetUp]
        public void Setup()
        {
            _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _planFileName = @"\PlanTextFile.txt";
        }

        [Test]
        public void ValidateFromTextFile()
        {
            _converterFromPlanTextToHtml = new UnicodeFileToHtmlTextConverter(_path + _planFileName);
            Assert.IsTrue(_converterFromPlanTextToHtml.Validate());
        }

        [Test]
        public void ReadFromTextFile()
        {
            _converterFromPlanTextToHtml = new UnicodeFileToHtmlTextConverter(_path + _planFileName);
            string _htmlContent = _converterFromPlanTextToHtml.ConvertToHtml("File title...");
            Debug.WriteLine("Plan text file content converted into html:");
            Debug.WriteLine(_htmlContent);
            Assert.IsTrue(_htmlContent != string.Empty);
        }

        [Test]
        public void ValidateFromString()
        {
            _converterFromPlanTextToHtml = new StringToHtmlTextConverter(File.OpenText(_path + _planFileName).ReadToEnd());
            Assert.IsTrue(_converterFromPlanTextToHtml.Validate());
        }

        [Test]
        public void ReadFromString()
        {
            _converterFromPlanTextToHtml = new StringToHtmlTextConverter(File.OpenText(_path + _planFileName).ReadToEnd());
            string _htmlContent = _converterFromPlanTextToHtml.ConvertToHtml("File title...");
            Debug.WriteLine("String converted into html:");
            Debug.WriteLine(_htmlContent);
            Assert.IsTrue(_htmlContent != string.Empty);
        }

        [Test]
        public void ValidateFromBytesArray()
        {
            _converterFromPlanTextToHtml = new ByteArrayToHtmlTextConverter(File.ReadAllBytes(_path + _planFileName));
            Assert.IsTrue(_converterFromPlanTextToHtml.Validate());
        }

        [Test]
        public void ReadFromBytesArray()
        {
            _converterFromPlanTextToHtml = new ByteArrayToHtmlTextConverter(File.ReadAllBytes(_path + _planFileName));
            string _htmlContent = _converterFromPlanTextToHtml.ConvertToHtml("File title...");
            Debug.WriteLine("String converted into html:");
            Debug.WriteLine(_htmlContent);
            Assert.IsTrue(_htmlContent != string.Empty);
        }

    }

}
