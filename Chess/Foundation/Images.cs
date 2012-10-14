using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Reflection;

namespace Chess.Foundation
{
    public class Images : VMBase
    {
        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                RaisePropertyChanged("Icon");
            }
        }

        private ImageSource _whitePawn;
        public ImageSource WhitePawn
        {
            get
            {
                return _whitePawn;
            }
            set
            {
                _whitePawn = value;
                RaisePropertyChanged("WhitePawn");
            }
        }

        private ImageSource _whiteRook;
        public ImageSource WhiteRook
        {
            get
            {
                return _whiteRook;
            }
            set
            {
                _whiteRook = value;
                RaisePropertyChanged("WhiteRook");
            }
        }

        private ImageSource _whiteBishop;
        public ImageSource WhiteBishop
        {
            get
            {
                return _whiteBishop;
            }
            set
            {
                _whiteBishop = value;
                RaisePropertyChanged("WhiteBishop");
            }
        }

        private ImageSource _whiteKnight;
        public ImageSource WhiteKnight
        {
            get
            {
                return _whiteKnight;
            }
            set
            {
                _whiteKnight = value;
                RaisePropertyChanged("WhiteKnight");
            }
        }
        
        private ImageSource _whiteQueen;
        public ImageSource WhiteQueen
        {
            get
            {
                return _whiteQueen;
            }
            set
            {
                _whiteQueen = value;
                RaisePropertyChanged("WhiteQueen");
            }
        }

        private ImageSource _whiteKing;
        public ImageSource WhiteKing
        {
            get
            {
                return _whiteKing;
            }
            set
            {
                _whiteKing = value;
                RaisePropertyChanged("WhiteKing");
            }
        }


        private ImageSource _blackPawn;
        public ImageSource BlackPawn
        {
            get
            {
                return _blackPawn;
            }
            set
            {
                _blackPawn = value;
                RaisePropertyChanged("BlackPawn");
            }
        }

        private ImageSource _blackRook;
        public ImageSource BlackRook
        {
            get
            {
                return _blackRook;
            }
            set
            {
                _blackRook = value;
                RaisePropertyChanged("BlackRook");
            }
        }

        private ImageSource _blackBishop;
        public ImageSource BlackBishop
        {
            get
            {
                return _blackBishop;
            }
            set
            {
                _blackBishop = value;
                RaisePropertyChanged("BlackBishop");
            }
        }

        private ImageSource _blackKnight;
        public ImageSource BlackKnight
        {
            get
            {
                return _blackKnight;
            }
            set
            {
                _blackKnight = value;
                RaisePropertyChanged("BlackKnight");
            }
        }

        private ImageSource _blackQueen;
        public ImageSource BlackQueen
        {
            get
            {
                return _blackQueen;
            }
            set
            {
                _blackQueen = value;
                RaisePropertyChanged("BlackQueen");
            }
        }

        private ImageSource _blackKing;
        public ImageSource BlackKing
        {
            get
            {
                return _blackKing;
            }
            set
            {
                _blackKing = value;
                RaisePropertyChanged("BlackKing");
            }
        }

        public Images()
        {
            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.WhitePawn.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.WhitePawn = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.WhiteRook.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.WhiteRook = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.WhiteBishop.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.WhiteBishop = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.WhiteKnight.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.WhiteKnight = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.WhiteQueen.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.WhiteQueen = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.WhiteKing.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.WhiteKing = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.BlackPawn.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.BlackPawn = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.BlackRook.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.BlackRook = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.BlackBishop.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.BlackBishop = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.BlackKnight.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.BlackKnight = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.BlackQueen.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.BlackQueen = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Images.BlackKing.png"))
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.BlackKing = decoder.Frames[0];
            }

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream("Chess.Icon.ico"))
            {
                IconBitmapDecoder decoder = new IconBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                this.Icon = decoder.Frames[0];
            }
        }
    }
}