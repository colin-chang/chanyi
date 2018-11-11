using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Chanyi.Shepherd.WPF.UserControls
{
    /// <summary>
    /// Loadding.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressRing : UserControl
    {
        public ProgressRing()
        {
            InitializeComponent();
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }
    }

    public class GifImage : UserControl
    {
        private GifAnimation gifAnimation = null;
        private Image image = null;

        public GifImage()
        {
        }

        public static readonly DependencyProperty ForceGifAnimProperty = DependencyProperty.Register("ForceGifAnim", typeof(bool), typeof(GifImage), new FrameworkPropertyMetadata(false));
        public bool ForceGifAnim
        {
            get
            {
                return (bool)this.GetValue(ForceGifAnimProperty);
            }
            set
            {
                this.SetValue(ForceGifAnimProperty, value);
            }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(GifImage), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSourceChanged)));
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GifImage obj = (GifImage)d;
            string s = (string)e.NewValue;
            obj.CreateFromSourceString(s);
        }
        public string Source
        {
            get
            {
                return (string)this.GetValue(SourceProperty);
            }
            set
            {
                this.SetValue(SourceProperty, value);
            }
        }


        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(GifImage), new FrameworkPropertyMetadata(Stretch.Fill, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnStretchChanged)));
        private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GifImage obj = (GifImage)d;
            Stretch s = (Stretch)e.NewValue;
            if (obj.gifAnimation != null)
            {
                obj.gifAnimation.Stretch = s;
            }
            else if (obj.image != null)
            {
                obj.image.Stretch = s;
            }
        }
        public Stretch Stretch
        {
            get
            {
                return (Stretch)this.GetValue(StretchProperty);
            }
            set
            {
                this.SetValue(StretchProperty, value);
            }
        }

        public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(GifImage), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnStretchDirectionChanged)));
        private static void OnStretchDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GifImage obj = (GifImage)d;
            StretchDirection s = (StretchDirection)e.NewValue;
            if (obj.gifAnimation != null)
            {
                obj.gifAnimation.StretchDirection = s;
            }
            else if (obj.image != null)
            {
                obj.image.StretchDirection = s;
            }
        }
        public StretchDirection StretchDirection
        {
            get
            {
                return (StretchDirection)this.GetValue(StretchDirectionProperty);
            }
            set
            {
                this.SetValue(StretchDirectionProperty, value);
            }
        }

        public delegate void ExceptionRoutedEventHandler(object sender, GifImageExceptionRoutedEventArgs args);

        public static readonly RoutedEvent ImageFailedEvent = EventManager.RegisterRoutedEvent("ImageFailed", RoutingStrategy.Bubble, typeof(ExceptionRoutedEventHandler), typeof(GifImage));

        public event ExceptionRoutedEventHandler ImageFailed
        {
            add
            {
                AddHandler(ImageFailedEvent, value);
            }
            remove
            {
                RemoveHandler(ImageFailedEvent, value);
            }
        }

        void image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            RaiseImageFailedEvent(e.ErrorException);
        }


        void RaiseImageFailedEvent(Exception exp)
        {
            GifImageExceptionRoutedEventArgs newArgs = new GifImageExceptionRoutedEventArgs(ImageFailedEvent, this);
            newArgs.ErrorException = exp;
            RaiseEvent(newArgs);
        }


        private void DeletePreviousImage()
        {
            if (image != null)
            {
                this.RemoveLogicalChild(image);
                image = null;
            }
            if (gifAnimation != null)
            {
                this.RemoveLogicalChild(gifAnimation);
                gifAnimation = null;
            }
        }

        private void CreateNonGifAnimationImage()
        {
            image = new Image();
            image.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(image_ImageFailed);
            ImageSource src = (ImageSource)(new ImageSourceConverter().ConvertFromString(Source));
            image.Source = src;
            image.Stretch = Stretch;
            image.StretchDirection = StretchDirection;
            this.AddChild(image);
        }


        private void CreateGifAnimation(MemoryStream memoryStream)
        {
            gifAnimation = new GifAnimation();
            gifAnimation.CreateGifAnimation(memoryStream);
            gifAnimation.Stretch = Stretch;
            gifAnimation.StretchDirection = StretchDirection;
            this.AddChild(gifAnimation);
        }


        private void CreateFromSourceString(string source)
        {
            DeletePreviousImage();
            Uri uri;

            try
            {
                uri = new Uri(source, UriKind.RelativeOrAbsolute);
            }
            catch (Exception exp)
            {
                RaiseImageFailedEvent(exp);
                return;
            }

            if (source.Trim().ToUpper().EndsWith(".GIF") || ForceGifAnim)
            {
                if (!uri.IsAbsoluteUri)
                {
                    GetGifStreamFromPack(uri);
                }
                else
                {

                    string leftPart = uri.GetLeftPart(UriPartial.Scheme);

                    if (leftPart == "http://" || leftPart == "ftp://" || leftPart == "file://")
                    {
                        GetGifStreamFromHttp(uri);
                    }
                    else if (leftPart == "pack://")
                    {
                        GetGifStreamFromPack(uri);
                    }
                    else
                    {
                        CreateNonGifAnimationImage();
                    }
                }
            }
            else
            {
                CreateNonGifAnimationImage();
            }
        }

        private delegate void WebRequestFinishedDelegate(MemoryStream memoryStream);

        private void WebRequestFinished(MemoryStream memoryStream)
        {
            CreateGifAnimation(memoryStream);
        }

        private delegate void WebRequestErrorDelegate(Exception exp);

        private void WebRequestError(Exception exp)
        {
            RaiseImageFailedEvent(exp);
        }

        private void WebResponseCallback(IAsyncResult asyncResult)
        {
            WebReadState webReadState = (WebReadState)asyncResult.AsyncState;
            WebResponse webResponse;
            try
            {
                webResponse = webReadState.webRequest.EndGetResponse(asyncResult);
                webReadState.readStream = webResponse.GetResponseStream();
                webReadState.buffer = new byte[100000];
                webReadState.readStream.BeginRead(webReadState.buffer, 0, webReadState.buffer.Length, new AsyncCallback(WebReadCallback), webReadState);
            }
            catch (WebException exp)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Render, new WebRequestErrorDelegate(WebRequestError), exp);
            }
        }

        private void WebReadCallback(IAsyncResult asyncResult)
        {
            WebReadState webReadState = (WebReadState)asyncResult.AsyncState;
            int count = webReadState.readStream.EndRead(asyncResult);
            if (count > 0)
            {
                webReadState.memoryStream.Write(webReadState.buffer, 0, count);
                try
                {
                    webReadState.readStream.BeginRead(webReadState.buffer, 0, webReadState.buffer.Length, new AsyncCallback(WebReadCallback), webReadState);
                }
                catch (WebException exp)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Render, new WebRequestErrorDelegate(WebRequestError), exp);
                }
            }
            else
            {
                this.Dispatcher.Invoke(DispatcherPriority.Render, new WebRequestFinishedDelegate(WebRequestFinished), webReadState.memoryStream);
            }
        }

        private void GetGifStreamFromHttp(Uri uri)
        {
            try
            {
                WebReadState webReadState = new WebReadState();
                webReadState.memoryStream = new MemoryStream();
                webReadState.webRequest = WebRequest.Create(uri);
                webReadState.webRequest.Timeout = 10000;

                webReadState.webRequest.BeginGetResponse(new AsyncCallback(WebResponseCallback), webReadState);
            }
            catch (SecurityException)
            {
                CreateNonGifAnimationImage();
            }
        }


        private void ReadGifStreamSynch(Stream s)
        {
            byte[] gifData;
            MemoryStream memoryStream;
            using (s)
            {
                memoryStream = new MemoryStream((int)s.Length);
                BinaryReader br = new BinaryReader(s);
                gifData = br.ReadBytes((int)s.Length);
                memoryStream.Write(gifData, 0, (int)s.Length);
                memoryStream.Flush();
            }
            CreateGifAnimation(memoryStream);
        }

        private void GetGifStreamFromPack(Uri uri)
        {
            try
            {
                StreamResourceInfo streamInfo;

                if (!uri.IsAbsoluteUri)
                {
                    streamInfo = Application.GetContentStream(uri);
                    if (streamInfo == null)
                    {
                        streamInfo = Application.GetResourceStream(uri);
                    }
                }
                else
                {
                    if (uri.GetLeftPart(UriPartial.Authority).Contains("siteoforigin"))
                    {
                        streamInfo = Application.GetRemoteStream(uri);
                    }
                    else
                    {
                        streamInfo = Application.GetContentStream(uri);
                        if (streamInfo == null)
                        {
                            streamInfo = Application.GetResourceStream(uri);
                        }
                    }
                }
                if (streamInfo == null)
                {
                    throw new FileNotFoundException("Resource not found.", uri.ToString());
                }
                ReadGifStreamSynch(streamInfo.Stream);
            }
            catch (Exception exp)
            {
                RaiseImageFailedEvent(exp);
            }
        }
    }

    public class GifImageExceptionRoutedEventArgs : RoutedEventArgs
    {
        public Exception ErrorException;

        public GifImageExceptionRoutedEventArgs(RoutedEvent routedEvent, object obj)
            : base(routedEvent, obj)
        {
        }
    }

    class WebReadState
    {
        public WebRequest webRequest;
        public MemoryStream memoryStream;
        public Stream readStream;
        public byte[] buffer;
    }

    class GifAnimation : Viewbox
    {

        private class GifFrame : Image
        {

            public int delayTime;

            public int disposalMethod;

            public int left;

            public int top;

            public int width;

            public int height;
        }

        private Canvas canvas = null;

        private List<GifFrame> frameList = null;

        private int frameCounter = 0;
        private int numberOfFrames = 0;

        private int numberOfLoops = -1;
        private int currentLoop = 0;

        private int logicalWidth = 0;
        private int logicalHeight = 0;

        private DispatcherTimer frameTimer = null;

        private GifFrame currentParseGifFrame;

        public GifAnimation()
        {
            canvas = new Canvas();
            this.Child = canvas;
        }

        private void Reset()
        {
            if (frameList != null)
            {
                frameList.Clear();
            }
            frameList = null;
            frameCounter = 0;
            numberOfFrames = 0;
            numberOfLoops = -1;
            currentLoop = 0;
            logicalWidth = 0;
            logicalHeight = 0;
            if (frameTimer != null)
            {
                frameTimer.Stop();
                frameTimer = null;
            }
        }

        #region PARSE
        private void ParseGif(byte[] gifData)
        {
            frameList = new List<GifFrame>();
            currentParseGifFrame = new GifFrame();
            ParseGifDataStream(gifData, 0);
        }


        private int ParseBlock(byte[] gifData, int offset)
        {
            switch (gifData[offset])
            {
                case 0x21:
                    if (gifData[offset + 1] == 0xF9)
                    {
                        return ParseGraphicControlExtension(gifData, offset);
                    }
                    else
                    {
                        return ParseExtensionBlock(gifData, offset);
                    }
                case 0x2C:
                    offset = ParseGraphicBlock(gifData, offset);
                    frameList.Add(currentParseGifFrame);
                    currentParseGifFrame = new GifFrame();
                    return offset;
                case 0x3B:
                    return -1;
                default:
                    throw new Exception("GIF format incorrect: missing graphic block or special-purpose block. ");
            }
        }

        private int ParseGraphicControlExtension(byte[] gifData, int offset)
        {
            int returnOffset = offset;
            int length = gifData[offset + 2];
            returnOffset = offset + length + 2 + 1;

            byte packedField = gifData[offset + 3];
            currentParseGifFrame.disposalMethod = (packedField & 0x1C) >> 2;

            int delay = BitConverter.ToUInt16(gifData, offset + 4);
            currentParseGifFrame.delayTime = delay;
            while (gifData[returnOffset] != 0x00)
            {
                returnOffset = returnOffset + gifData[returnOffset] + 1;
            }

            returnOffset++;

            return returnOffset;
        }

        private int ParseLogicalScreen(byte[] gifData, int offset)
        {
            logicalWidth = BitConverter.ToUInt16(gifData, offset);
            logicalHeight = BitConverter.ToUInt16(gifData, offset + 2);

            byte packedField = gifData[offset + 4];
            bool hasGlobalColorTable = (int)(packedField & 0x80) > 0 ? true : false;

            int currentIndex = offset + 7;
            if (hasGlobalColorTable)
            {
                int colorTableLength = packedField & 0x07;
                colorTableLength = (int)Math.Pow(2, colorTableLength + 1) * 3;
                currentIndex = currentIndex + colorTableLength;
            }
            return currentIndex;
        }

        private int ParseGraphicBlock(byte[] gifData, int offset)
        {
            currentParseGifFrame.left = BitConverter.ToUInt16(gifData, offset + 1);
            currentParseGifFrame.top = BitConverter.ToUInt16(gifData, offset + 3);
            currentParseGifFrame.width = BitConverter.ToUInt16(gifData, offset + 5);
            currentParseGifFrame.height = BitConverter.ToUInt16(gifData, offset + 7);
            if (currentParseGifFrame.width > logicalWidth)
            {
                logicalWidth = currentParseGifFrame.width;
            }
            if (currentParseGifFrame.height > logicalHeight)
            {
                logicalHeight = currentParseGifFrame.height;
            }
            byte packedField = gifData[offset + 9];
            bool hasLocalColorTable = (int)(packedField & 0x80) > 0 ? true : false;

            int currentIndex = offset + 9;
            if (hasLocalColorTable)
            {
                int colorTableLength = packedField & 0x07;
                colorTableLength = (int)Math.Pow(2, colorTableLength + 1) * 3;
                currentIndex = currentIndex + colorTableLength;
            }
            currentIndex++;

            currentIndex++;

            while (gifData[currentIndex] != 0x00)
            {
                int length = gifData[currentIndex];
                currentIndex = currentIndex + gifData[currentIndex];
                currentIndex++;
            }
            currentIndex = currentIndex + 1;
            return currentIndex;
        }

        private int ParseExtensionBlock(byte[] gifData, int offset)
        {
            int returnOffset = offset;
            int length = gifData[offset + 2];
            returnOffset = offset + length + 2 + 1;
            if (gifData[offset + 1] == 0xFF && length > 10)
            {
                string netscape = System.Text.ASCIIEncoding.ASCII.GetString(gifData, offset + 3, 8);
                if (netscape == "NETSCAPE")
                {
                    numberOfLoops = BitConverter.ToUInt16(gifData, offset + 16);
                    if (numberOfLoops > 0)
                    {
                        numberOfLoops++;
                    }
                }
            }
            while (gifData[returnOffset] != 0x00)
            {
                returnOffset = returnOffset + gifData[returnOffset] + 1;
            }

            returnOffset++;

            return returnOffset;
        }

        private int ParseHeader(byte[] gifData, int offset)
        {
            string str = System.Text.ASCIIEncoding.ASCII.GetString(gifData, offset, 3);
            if (str != "GIF")
            {
                throw new Exception("Not a proper GIF file: missing GIF header");
            }
            return 6;
        }

        private void ParseGifDataStream(byte[] gifData, int offset)
        {
            offset = ParseHeader(gifData, offset);
            offset = ParseLogicalScreen(gifData, offset);
            while (offset != -1)
            {
                offset = ParseBlock(gifData, offset);
            }
        }

        #endregion

        public void CreateGifAnimation(MemoryStream memoryStream)
        {
            Reset();

            byte[] gifData = memoryStream.GetBuffer();  // Use GetBuffer so that there is no memory copy

            GifBitmapDecoder decoder = new GifBitmapDecoder(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            numberOfFrames = decoder.Frames.Count;

            try
            {
                ParseGif(gifData);
            }
            catch
            {
                throw new FileFormatException("Unable to parse Gif file format.");
            }

            for (int i = 0; i < decoder.Frames.Count; i++)
            {
                frameList[i].Source = decoder.Frames[i];
                frameList[i].Visibility = Visibility.Hidden;
                canvas.Children.Add(frameList[i]);
                Canvas.SetLeft(frameList[i], frameList[i].left);
                Canvas.SetTop(frameList[i], frameList[i].top);
                Canvas.SetZIndex(frameList[i], i);
            }
            canvas.Height = logicalHeight;
            canvas.Width = logicalWidth;

            frameList[0].Visibility = Visibility.Visible;

            for (int i = 0; i < frameList.Count; i++)
            {
                Console.WriteLine(frameList[i].disposalMethod.ToString() + " " + frameList[i].width.ToString() + " " + frameList[i].delayTime.ToString());
            }

            if (frameList.Count > 1)
            {
                if (numberOfLoops == -1)
                {
                    numberOfLoops = 1;
                }
                frameTimer = new System.Windows.Threading.DispatcherTimer();
                frameTimer.Tick += NextFrame;
                frameTimer.Interval = new TimeSpan(0, 0, 0, 0, frameList[0].delayTime * 10);
                frameTimer.Start();
            }
        }

        public void NextFrame()
        {
            NextFrame(null, null);
        }

        public void NextFrame(object sender, EventArgs e)
        {
            frameTimer.Stop();
            if (numberOfFrames == 0) return;
            if (frameList[frameCounter].disposalMethod == 2)
            {
                frameList[frameCounter].Visibility = Visibility.Hidden;
            }
            if (frameList[frameCounter].disposalMethod >= 3)
            {
                frameList[frameCounter].Visibility = Visibility.Hidden;
            }
            frameCounter++;

            if (frameCounter < numberOfFrames)
            {
                frameList[frameCounter].Visibility = Visibility.Visible;
                frameTimer.Interval = new TimeSpan(0, 0, 0, 0, frameList[frameCounter].delayTime * 10);
                frameTimer.Start();
            }
            else
            {
                if (numberOfLoops != 0)
                {
                    currentLoop++;
                }
                if (currentLoop < numberOfLoops || numberOfLoops == 0)
                {
                    for (int f = 0; f < frameList.Count; f++)
                    {
                        frameList[f].Visibility = Visibility.Hidden;
                    }
                    frameCounter = 0;
                    frameList[frameCounter].Visibility = Visibility.Visible;
                    frameTimer.Interval = new TimeSpan(0, 0, 0, 0, frameList[frameCounter].delayTime * 10);
                    frameTimer.Start();
                }
            }
        }
    }
}
