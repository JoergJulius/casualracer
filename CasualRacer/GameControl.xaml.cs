using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CasualRacer.Model;

namespace CasualRacer
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        private Game game = new Game();
        private DispatcherTimer timer = new DispatcherTimer();
        private Stopwatch totalWatch = new Stopwatch();
        private Stopwatch elapsedWatch = new Stopwatch();

        private ImageBrush dirtBrush;
        private ImageBrush sandBrush;
        private ImageBrush grasBrush;
        private ImageBrush roadBrush;
        private ImageBrush tilesBrush;

        public GameControl()
        {
            InitializeComponent();
            DataContext = game;

            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += Timer_Tick;
            timer.IsEnabled = true;

            totalWatch.Start();
            elapsedWatch.Start();

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.KeyUp += MainWindow_KeyUp;

            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets");
            dirtBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\dirt_center.png")));
            sandBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\sand_center.png")));
            grasBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\grass_center.png")));
            roadBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\asphalt_center.png")));

            tilesBrush=new ImageBrush(new BitmapImage(new Uri(path + "\\tiles.png")));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = elapsedWatch.Elapsed;
            elapsedWatch.Restart();
            game.Update(totalWatch.Elapsed, elapsedWatch.Elapsed);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch( e.Key )
            {
                case Key.Up:
                    game.Player1.Acceleration = false;
                    break;
                case Key.Left:
                    game.Player1.WheelLeft = false;
                    break;
                case Key.Right:
                    game.Player1.WheelRight = false;
                    break;
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch( e.Key )
            {
                case Key.Up:
                    game.Player1.Acceleration = true;
                    break;
                case Key.Left:
                    game.Player1.WheelLeft = true;
                    break;
                case Key.Right:
                    game.Player1.WheelRight = true;
                    break;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Track track = (DataContext as Game).Track;

            tilesBrush.TileMode = TileMode.Tile;
            tilesBrush.Viewport = new Rect(0, 0, 0.5f / track.Tiles.GetLength(0), 0.5f / track.Tiles.GetLength(1));
            tilesBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;

            tilesBrush.Viewbox = new Rect(1820, 0, 128, 128);
            tilesBrush.ViewboxUnits = BrushMappingMode.Absolute;
            drawingContext.DrawRectangle(tilesBrush, null, new Rect(0, 0, Track.CELLSIZE* track.Tiles.GetLength(0), Track.CELLSIZE* track.Tiles.GetLength(1)));

            for( int x = 0; x < track.Tiles.GetLength(0); x++ )
            {
                for( int y = 0; y < track.Tiles.GetLength(1); y++ )
                {
                    switch( track.Tiles[x, y] )
                    {
                        case TrackTile.Gras:
                            DrawGras(x, y);
                            break;
                        case TrackTile.Road:
                            break;
                        case TrackTile.Sand:
                            break;
                    }
                    //drawingContext.DrawRectangle(brush, null, new Rect(x * Track.CELLSIZE, y * Track.CELLSIZE, Track.CELLSIZE, Track.CELLSIZE));
                }
            }
        }

        private void DrawGras(int x, int y)
        {
            //linke obere ecke
            if( game.Track.Tiles[x-1,y]!=TrackTile.Gras )
            {
                if( game.Track.Tiles[x, y-1]!=TrackTile.Gras )
                {
                    //konvexe linke ecke
                }
                else
                {
                    //linke ecke
                }
            }
            else
            {
                if( game.Track.Tiles[x, y-1]!=TrackTile.Gras )
                {
                    //obere kante
                }
                else
                {
                    if( game.Track.Tiles[x-1,y-1]!=TrackTile.Gras )
                    {
                        //linke conkave ecke
                    }
                    else
                    {
                        //vollflächig gras
                    }
                }
            }
        }
    }
}
