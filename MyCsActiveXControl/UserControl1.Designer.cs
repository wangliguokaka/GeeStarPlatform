namespace MyCsActiveXControl
{
    partial class UserControl1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.MessageButton = new System.Windows.Forms.Button();
            this.JsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MessageButton
            // 
            this.MessageButton.Location = new System.Drawing.Point(3, 3);
            this.MessageButton.Name = "MessageButton";
            this.MessageButton.Size = new System.Drawing.Size(109, 23);
            this.MessageButton.TabIndex = 0;
            this.MessageButton.Text = "当前的Message";
            this.MessageButton.UseVisualStyleBackColor = true;
            this.MessageButton.Click += new System.EventHandler(this.MessageButton_Click);
            // 
            // JsButton
            // 
            this.JsButton.Location = new System.Drawing.Point(3, 32);
            this.JsButton.Name = "JsButton";
            this.JsButton.Size = new System.Drawing.Size(109, 23);
            this.JsButton.TabIndex = 1;
            this.JsButton.Text = "执行JavaScript";
            this.JsButton.UseVisualStyleBackColor = true;
            this.JsButton.Click += new System.EventHandler(this.JsButton_Click);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.JsButton);
            this.Controls.Add(this.MessageButton);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(120, 62);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button MessageButton;
        private System.Windows.Forms.Button JsButton;
    }
}
