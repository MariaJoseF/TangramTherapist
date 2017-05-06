using System;
using UnityEngine;
using System.Windows.Forms;
using System.IO;

namespace Assets.Scripts.Learning
{
    public class Ratings : MonoBehaviour
    {
        private System.Windows.Forms.Button Bt_1;
        private System.Windows.Forms.Button Bt_2;
        private System.Windows.Forms.Button Bt_3;
        private System.Windows.Forms.Button Bt_4;
        private System.Windows.Forms.Button Bt_5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public Form form_Feedback;

        public Button Button_1
        {
            get
            {
                return Bt_1;
            }

            set
            {
                Bt_1 = value;
            }
        }

        public Button Button_2
        {
            get
            {
                return Bt_2;
            }

            set
            {
                Bt_2 = value;
            }
        }

        public Button Button_3
        {
            get
            {
                return Bt_3;
            }

            set
            {
                Bt_3 = value;
            }
        }

        public Button Button_4
        {
            get
            {
                return Bt_4;
            }

            set
            {
                Bt_4 = value;
            }
        }

        public Button Button_5
        {
            get
            {
                return Bt_5;
            }

            set
            {
                Bt_5 = value;
            }
        }

        public Ratings()
        {
            form_Feedback = new Form();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>

            this.Bt_1 = new System.Windows.Forms.Button();
            this.Bt_2 = new System.Windows.Forms.Button();
            this.Bt_3 = new System.Windows.Forms.Button();
            this.Bt_4 = new System.Windows.Forms.Button();
            this.Bt_5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            //  this.SuspendLayout();
            // 
            // Bt_1
            // 
            this.Bt_1.Location = new System.Drawing.Point(119, 142);
            this.Bt_1.Name = "Bt_1";
            this.Bt_1.Size = new System.Drawing.Size(130, 90);
            this.Bt_1.TabIndex = 0;
            this.Bt_1.Text = "1";
            this.Bt_1.UseVisualStyleBackColor = true;
            this.Bt_1.Visible = false;
            this.Bt_1.Click += new System.EventHandler(this.Bt_1_Click);
            // 
            // Bt_2
            // 
            this.Bt_2.Location = new System.Drawing.Point(312, 142);
            this.Bt_2.Name = "Bt_2";
            this.Bt_2.Size = new System.Drawing.Size(130, 90);
            this.Bt_2.TabIndex = 1;
            this.Bt_2.Text = "2";
            this.Bt_2.UseVisualStyleBackColor = true;
            this.Bt_2.Visible = false;
            this.Bt_2.Click += new System.EventHandler(this.Bt_2_Click);
            // 
            // Bt_3
            // 
            this.Bt_3.Location = new System.Drawing.Point(509, 142);
            this.Bt_3.Name = "Bt_3";
            this.Bt_3.Size = new System.Drawing.Size(130, 90);
            this.Bt_3.TabIndex = 2;
            this.Bt_3.Text = "3";
            this.Bt_3.UseVisualStyleBackColor = true;
            this.Bt_3.Visible = false;
            this.Bt_3.Click += new System.EventHandler(this.Bt_3_Click);
            // 
            // Bt_4
            // 
            this.Bt_4.Location = new System.Drawing.Point(698, 142);
            this.Bt_4.Name = "Bt_4";
            this.Bt_4.Size = new System.Drawing.Size(130, 90);
            this.Bt_4.TabIndex = 3;
            this.Bt_4.Text = "4";
            this.Bt_4.UseVisualStyleBackColor = true;
            this.Bt_4.Visible = false;
            this.Bt_4.Click += new System.EventHandler(this.Bt_4_Click);
            // 
            // Bt_5
            // 
            this.Bt_5.Location = new System.Drawing.Point(884, 142);
            this.Bt_5.Name = "Bt_5";
            this.Bt_5.Size = new System.Drawing.Size(130, 90);
            this.Bt_5.TabIndex = 4;
            this.Bt_5.Text = "5";
            this.Bt_5.UseVisualStyleBackColor = true;
            this.Bt_5.Visible = false;
            this.Bt_5.Click += new System.EventHandler(this.Bt_5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(471, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Feedback Ratings";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Very dissatisfied ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(879, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Very satisfied ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(531, 274);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Neither";
            // 
            // Ratings
            // 

            this.form_Feedback.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.form_Feedback.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.form_Feedback.ClientSize = new System.Drawing.Size(1123, 367);
            this.form_Feedback.Controls.Add(this.label4);
            this.form_Feedback.Controls.Add(this.label3);
            this.form_Feedback.Controls.Add(this.label2);
            this.form_Feedback.Controls.Add(this.label1);
            this.form_Feedback.Controls.Add(this.Bt_5);
            this.form_Feedback.Controls.Add(this.Bt_4);
            this.form_Feedback.Controls.Add(this.Bt_3);
            this.form_Feedback.Controls.Add(this.Bt_2);
            this.form_Feedback.Controls.Add(this.Bt_1);
            this.form_Feedback.Name = "Ratings";
            this.form_Feedback.Text = "Ratings";
            this.form_Feedback.ResumeLayout(false);
            this.form_Feedback.PerformLayout();

        }


        private void Bt_1_Click(object sender, EventArgs e)
        {
            WriteJSON("", "BT1");
        }

        private void Bt_2_Click(object sender, EventArgs e)
        {
            WriteJSON("", "BT2");
        }

        private void Bt_3_Click(object sender, EventArgs e)
        {
            WriteJSON("", "BT3");
        }

        private void Bt_4_Click(object sender, EventArgs e)
        {
            WriteJSON("", "BT4");
        }

        private void Bt_5_Click(object sender, EventArgs e)
        {
            WriteJSON("", "BT5");
        }

        private void WriteJSON(string timestamp, string info)
        {

            string filePath = @"c:\Developer\Logs\TEST\";
            Console.WriteLine(filePath);
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    Console.WriteLine(filePath);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            
            string fileName = null;
            if (fileName == null)
            {
                fileName = filePath + @"ucb_" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt";
                Console.WriteLine(filePath + @"\" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt");
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(fileName))
                {

                    file.WriteLine(timestamp + " " + info);
                }
            }
            else
            {
                using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(fileName, true))
                {

                    file.WriteLine(timestamp + " " + info);
                }
            }
        }
    }
}
