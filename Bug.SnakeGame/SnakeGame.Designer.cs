namespace Bug.SnakeGame
{
    partial class SnakeGame
	{
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			ptbPlayGround = new PictureBox();
			clock = new System.Windows.Forms.Timer(components);
			((System.ComponentModel.ISupportInitialize)ptbPlayGround).BeginInit();
			SuspendLayout();
			// 
			// ptbPlayGround
			// 
			ptbPlayGround.Dock = DockStyle.Fill;
			ptbPlayGround.Location = new Point(0, 0);
			ptbPlayGround.Name = "ptbPlayGround";
			ptbPlayGround.Size = new Size(500, 500);
			ptbPlayGround.TabIndex = 0;
			ptbPlayGround.TabStop = false;
			// 
			// clock
			// 
			clock.Interval = 200;
			clock.Tick += GameLoop;
			// 
			// SnakeGame
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(500, 500);
			Controls.Add(ptbPlayGround);
			Name = "SnakeGame";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Snake Game";
			((System.ComponentModel.ISupportInitialize)ptbPlayGround).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private PictureBox ptbPlayGround;
		private System.Windows.Forms.Timer clock;
	}
}
