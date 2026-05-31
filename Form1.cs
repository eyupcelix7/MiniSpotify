namespace MiniSpotify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - 390, 15);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Win32.SetRoundedCorner(Handle);
        }
    }
}
