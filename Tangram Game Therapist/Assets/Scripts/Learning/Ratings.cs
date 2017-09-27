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

        private int ActionNumber;
        private string fileName = null;
        public Form form_Feedback;
        internal bool header = true;

        public int previousAction = -1;
        internal double feedback_val = -1.0f;
        internal int default_form = 1;

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

        public Label Label1
        {
            get
            {
                return label1;
            }

            set
            {
                label1 = value;
            }
        }

        public int ActionNumber1
        {
            get
            {
                return ActionNumber;
            }

            set
            {
                ActionNumber = value;
            }
        }

        public Ratings()
        {
            form_Feedback = new Form();
            InitializeComponent();
            ActionNumber = -1;
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
            this.Bt_1.Enabled = false;
            this.Bt_1.Click += new System.EventHandler(this.Bt_1_Click);

            this.Bt_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bt_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // Bt_2
            // 
            this.Bt_2.Location = new System.Drawing.Point(312, 142);
            this.Bt_2.Name = "Bt_2";
            this.Bt_2.Size = new System.Drawing.Size(130, 90);
            this.Bt_2.TabIndex = 1;
            this.Bt_2.Text = "2";
            this.Bt_2.UseVisualStyleBackColor = true;
            this.Bt_2.Enabled = false;
            this.Bt_2.Click += new System.EventHandler(this.Bt_2_Click);

            this.Bt_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bt_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // Bt_3
            // 
            this.Bt_3.Location = new System.Drawing.Point(509, 142);
            this.Bt_3.Name = "Bt_3";
            this.Bt_3.Size = new System.Drawing.Size(130, 90);
            this.Bt_3.TabIndex = 2;
            this.Bt_3.Text = "3";
            this.Bt_3.UseVisualStyleBackColor = true;
            this.Bt_3.Enabled = false;
            this.Bt_3.Click += new System.EventHandler(this.Bt_3_Click);

            this.Bt_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bt_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // Bt_4
            // 
            this.Bt_4.Location = new System.Drawing.Point(698, 142);
            this.Bt_4.Name = "Bt_4";
            this.Bt_4.Size = new System.Drawing.Size(130, 90);
            this.Bt_4.TabIndex = 3;
            this.Bt_4.Text = "4";
            this.Bt_4.UseVisualStyleBackColor = true;
            this.Bt_4.Enabled = false;
            this.Bt_4.Click += new System.EventHandler(this.Bt_4_Click);

            this.Bt_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bt_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // Bt_5
            // 
            this.Bt_5.Location = new System.Drawing.Point(884, 142);
            this.Bt_5.Name = "Bt_5";
            this.Bt_5.Size = new System.Drawing.Size(130, 90);
            this.Bt_5.TabIndex = 4;
            this.Bt_5.Text = "5";
            this.Bt_5.UseVisualStyleBackColor = true;
            this.Bt_5.Enabled = false;
            this.Bt_5.Click += new System.EventHandler(this.Bt_5_Click);

            this.Bt_5.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Bt_5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

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

            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "   Very badly (-) ";

            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(879, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "   Very well (+)";

            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(531, 274);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Neither";

            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            //this.form_Feedback.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FeedbackForm_FormClosing);
            this.form_Feedback.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.form_Feedback.PerformLayout();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("The game will be closed, are you sure?", "Exit Game", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                UtterancesManager.Instance.WriteJSON("Feedback form was closed by user/rating person and the game was forced to be finished.");
                Debug.Log("Game is exiting");
                System.Windows.Forms.Application.Exit();
                UnityEngine.Application.Quit();
            }
            else if (dialogResult == DialogResult.No)
            {
                Debug.Log("Try to exit Game");
            }

        }

        //private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
        //    messageBoxCS.AppendFormat("{0} = {1}", "CloseReason", e.CloseReason);
        //    messageBoxCS.AppendLine();
        //    messageBoxCS.AppendFormat("{0} = {1}", "Cancel", e.CloseReason);
        //    messageBoxCS.AppendLine();
        //    MessageBox.Show(messageBoxCS.ToString(), "Form1_FormClosed Event");
        //}

        private void Bt_1_Click(object sender, EventArgs e)
        {
            FileHeader();
            WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + ActionNumber + ";" + "1");
            ButtonsDesactivation();
            header = false;
            previousAction = ActionNumber;
            feedback_val = 1.0f;
            default_form = 0;

            //Therapist.Instance.vec_ratings.Add(1);
            Therapist.Instance.AlgorithmEXP3.UpdateReward(previousAction, Math.Round(feedback_val, 2));

        }

        private void Bt_2_Click(object sender, EventArgs e)
        {
            FileHeader();
            WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + ActionNumber + ";" + "2");
            ButtonsDesactivation();
            header = false;
            previousAction = ActionNumber;
            feedback_val = 2.0f;
            default_form = 0;

            //Therapist.Instance.vec_ratings.Add(2);
            Therapist.Instance.AlgorithmEXP3.UpdateReward(previousAction, Math.Round(feedback_val, 2));

        }

        private void Bt_3_Click(object sender, EventArgs e)
        {
            FileHeader();
            WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + ActionNumber + ";" + "3");
            ButtonsDesactivation();
            header = false;
            previousAction = ActionNumber;
            feedback_val = 3.0f;
            default_form = 0;

            //Therapist.Instance.vec_ratings.Add(3);
            Therapist.Instance.AlgorithmEXP3.UpdateReward(previousAction, Math.Round(feedback_val, 2));

        }

        private void Bt_4_Click(object sender, EventArgs e)
        {
            FileHeader();
            WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + ActionNumber + ";" + "4");
            ButtonsDesactivation();
            header = false;
            previousAction = ActionNumber;
            feedback_val = 4.0f;
            default_form = 0;

            //Therapist.Instance.vec_ratings.Add(4);
            Therapist.Instance.AlgorithmEXP3.UpdateReward(previousAction, Math.Round(feedback_val, 2));

        }

        private void Bt_5_Click(object sender, EventArgs e)
        {
            FileHeader();
            WriteJSON(DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), ";" + GameManager.Instance.playerName + ";" + GameManager.Instance.CurrentPuzzle + ";" + GameManager.Instance.Difficulty_ + ";" + GameManager.Instance.RotationMode_ + ";" + GameManager.Instance.DistanceThreshold + ";" + ActionNumber + ";" + "5");
            ButtonsDesactivation();
            header = false;
            previousAction = ActionNumber;
            feedback_val = 5.0f;
            default_form = 0;

           // Therapist.Instance.vec_ratings.Add(5);
            Therapist.Instance.AlgorithmEXP3.UpdateReward(previousAction, Math.Round(feedback_val, 2));
            
        }

        internal bool CheckBts()
        {
            if (Bt_1.Enabled == true && Bt_2.Enabled == true && Bt_3.Enabled == true && Bt_4.Enabled == true && Bt_5.Enabled == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void ButtonsDesactivation()
        {
            Bt_1.Enabled = false;
            Bt_2.Enabled = false;
            Bt_3.Enabled = false;
            Bt_4.Enabled = false;
            Bt_5.Enabled = false;

            Bt_1.BackColor = System.Drawing.SystemColors.Control;
            Bt_2.BackColor = System.Drawing.SystemColors.Control;
            Bt_3.BackColor = System.Drawing.SystemColors.Control;
            Bt_4.BackColor = System.Drawing.SystemColors.Control;
            Bt_5.BackColor = System.Drawing.SystemColors.Control;
        }

        public void FileHeader()
        {
            if (header)
            {
                WriteJSON("", "DATE/TIME;PLAYER;PUZZLE;DIFICULDADE;MODO_ROTAÇAO;THRESHOLD;ACTION;FEEDBACK_ID");
            }

        }

        internal void WriteJSON(string timestamp, string info)
        {

            string filePath = @"c:\Developer\Logs\Feedback\";
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

            if (fileName == null)
            {
                fileName = filePath + @"feedback_" + String.Format("{0:dd-MM-yyyy hh-mm-ss}", DateTime.Now) + ".txt";
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
