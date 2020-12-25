namespace WallpaperManager.Tagging
{
    partial class TagContainer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.labelPageNumber = new System.Windows.Forms.Label();
            this.tagContainerFLP = new System.Windows.Forms.FlowLayoutPanel();
            this.textBoxSearchTag = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSort = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRight
            // 
            this.buttonRight.Location = new System.Drawing.Point(475, 165);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(20, 20);
            this.buttonRight.TabIndex = 34;
            this.buttonRight.Text = ">";
            this.buttonRight.UseVisualStyleBackColor = true;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.Location = new System.Drawing.Point(5, 165);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(20, 20);
            this.buttonLeft.TabIndex = 35;
            this.buttonLeft.Text = "<";
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // labelPageNumber
            // 
            this.labelPageNumber.AutoSize = true;
            this.labelPageNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPageNumber.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelPageNumber.Location = new System.Drawing.Point(6, 32);
            this.labelPageNumber.Name = "labelPageNumber";
            this.labelPageNumber.Size = new System.Drawing.Size(19, 20);
            this.labelPageNumber.TabIndex = 36;
            this.labelPageNumber.Text = "1";
            // 
            // tagContainerFLP
            // 
            this.tagContainerFLP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.tagContainerFLP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tagContainerFLP.Location = new System.Drawing.Point(31, 32);
            this.tagContainerFLP.Name = "tagContainerFLP";
            this.tagContainerFLP.Size = new System.Drawing.Size(438, 318);
            this.tagContainerFLP.TabIndex = 37;
            // 
            // textBoxSearchTag
            // 
            this.textBoxSearchTag.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.textBoxSearchTag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearchTag.ForeColor = System.Drawing.SystemColors.Info;
            this.textBoxSearchTag.Location = new System.Drawing.Point(56, 6);
            this.textBoxSearchTag.Name = "textBoxSearchTag";
            this.textBoxSearchTag.Size = new System.Drawing.Size(379, 20);
            this.textBoxSearchTag.TabIndex = 38;
            this.textBoxSearchTag.WordWrap = false;
            this.textBoxSearchTag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearchTag_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Search:";
            // 
            // buttonSort
            // 
            this.buttonSort.Location = new System.Drawing.Point(441, 3);
            this.buttonSort.Name = "buttonSort";
            this.buttonSort.Size = new System.Drawing.Size(54, 23);
            this.buttonSort.TabIndex = 40;
            this.buttonSort.Text = "Sort";
            this.buttonSort.UseVisualStyleBackColor = true;
            this.buttonSort.Click += new System.EventHandler(this.buttonSort_Click);
            // 
            // TagContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.Controls.Add(this.buttonSort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSearchTag);
            this.Controls.Add(this.tagContainerFLP);
            this.Controls.Add(this.labelPageNumber);
            this.Controls.Add(this.buttonLeft);
            this.Controls.Add(this.buttonRight);
            this.Name = "TagContainer";
            this.Size = new System.Drawing.Size(500, 350);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Label labelPageNumber;
        private System.Windows.Forms.FlowLayoutPanel tagContainerFLP;
        private System.Windows.Forms.TextBox textBoxSearchTag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSort;
    }
}
