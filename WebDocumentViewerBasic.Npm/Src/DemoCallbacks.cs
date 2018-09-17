using System.Diagnostics;
using System.IO;
using Atalasoft.Imaging.Codec;
using Atalasoft.Imaging.Text;
using Atalasoft.Imaging.WebControls;
using Atalasoft.Imaging.WebControls.Core;
using Atalasoft.Imaging.WebControls.Text;

namespace WebDocumentViewerBasic.Npm
{
    public class DemoCallbacks : WebDocumentViewerCallbacks
    {
        private readonly string _webRoot;
        public DemoCallbacks(string webRootPath)
        {
            this._webRoot = webRootPath;
        }

        public override void PageTextRequested(PageTextRequestedEventArgs args)
        {
            
            var serverPath = Path.Combine(this._webRoot,args.FilePath);
            if (!File.Exists(serverPath))
                return;

            using (var stream = File.OpenRead(serverPath))
            {
                try
                {
                    if (!(RegisteredDecoders.GetDecoder(stream) is ITextFormatDecoder decoder))
                        return;

                    using (var extractor = new SegmentedTextTranslator(decoder.GetTextDocument(stream)))
                    {
                        // for documents that have comlicated structure, i.e. consist from the isolated pieces of text, or table structure
                        // it's possible to configure nearby text blocks are combined into text segments(text containers that provide 
                        // selection isolated from other document content)
                        extractor.RegionDetection = TextRegionDetectionMode.BlockDetection;

                        // each block boundaries inflated to one average character width and two average character height 
                        // and all intersecting blocks are combined into single segment.
                        // Having vertical ratio bigger then horizontal behaves better on column-layout documents.
                        extractor.BlockDetectionDistance = new System.Drawing.SizeF(1, 2);
                        args.Page = extractor.ExtractPageText(args.Index);
                    }
                }
                catch (ImageReadException imagingException)
                {
                    Debug.WriteLine("Text extraction: image type is not recognized. {0}", imagingException);
                }
            }
        }
    }
}
