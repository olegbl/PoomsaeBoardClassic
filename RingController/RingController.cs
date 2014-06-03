using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public partial class RingControllerForm : Form
    {
        public static IPAddress GetIPAddress()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Environment.MachineName);
            foreach (IPAddress address in hostEntry.AddressList)
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    return address;
            return null;
        }

        private StringServer server;
        private List<Display> windows;

        private Display.Modes mode;
        public Display.Modes Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;

                if (this.mode == Display.Modes.REGULAR_POOMSAE)
                {
                    this.poomsae_control.Visible = false;
                    this.regular_radio.Checked = true;
                    this.sports_radio.Checked = false;
                    this.PoomsaeNumber = 1;
                }

                if (this.mode == Display.Modes.SPORTS_POOMSAE)
                {
                    this.poomsae_control.Visible = true;
                    this.regular_radio.Checked = false;
                    this.sports_radio.Checked = true;
                }

                foreach (Display window in windows)
                {
                    window.Mode = this.mode;
                    window.redraw();
                }
            }
        }
        public Display.Modes PoomsaeMode
        {
            get
            {
                if (this.sports_radio.Checked) return Display.Modes.SPORTS_POOMSAE;
                if (this.regular_radio.Checked) return Display.Modes.REGULAR_POOMSAE;
                return Display.Modes.TIMER;
            }
        }

        private List<Judge> judges;
        public List<Judge> Judges
        {
            get
            {
                return this.judges;
            }
        }

        private Poomsae poomsae1;
        public Poomsae Poomsae1
        {
            get
            {
                return this.poomsae1;
            }
        }

        private Poomsae poomsae2;
        public Poomsae Poomsae2
        {
            get
            {
                return this.poomsae2;
            }
        }

        public Poomsae Poomsae
        {
            get
            {
                if (this.poomsae1_radio.Checked) return this.poomsae1;
                if (this.poomsae2_radio.Checked) return this.poomsae2;
                return null;
            }
        }

        public int PoomsaeNumber
        {
            get
            {
                if (this.poomsae1_radio.Checked) return 1;
                if (this.poomsae2_radio.Checked) return 2;
                return 0;
            }
            set
            {
                if (value == 1 || value == -1)
                {
                    if (value > 0) this.poomsae1_radio.Checked = true;
                    this.poomsae2_radio.Checked = false;
                }

                if (value == 2 || value == -2)
                {
                    this.poomsae1_radio.Checked = false;
                    if (value > 0) this.poomsae2_radio.Checked = true;
                }

                foreach (Judge judge in judges)
                {
                    MessageService.sendMessage(judge.Client, "clear");
                    MessageService.sendMessage(judge.Client, "name", this.AthleteName);
                    MessageService.sendMessage(judge.Client, "poomsae", Math.Abs(value));
                }
            }
        }

        public String Score
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.score_final.Text, out temp);
                return String.Format("{0:0.0000}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setText(this.score_final, String.Format("{0:0.0000}", temp));

            }
        }

        public RingControllerForm()
        {
            this.server = new StringServer();
            this.judges = new List<Judge>();
            this.windows = new List<Display>();

            InitializeComponent();

            this.Width = 290;
            this.Height = 150;

            this.poomsae1 = new Poomsae(this.score1_time, this.score1_technical, this.score1_presentation, this.score1_total);
            this.poomsae2 = new Poomsae(this.score2_time, this.score2_technical, this.score2_presentation, this.score2_total);

            this.Mode = Display.Modes.SPORTS_POOMSAE;
        }

        private delegate void RingNumberDelegate(String value);
        private void RingNumberSetter(String value) { this.RingNumber = value; }
        public String RingNumber
        {
            get
            {
                return ring_textbox.Text;
            }
            set
            {
                if (this.ring_textbox.InvokeRequired)
                    this.ring_textbox.Invoke(new RingNumberDelegate(RingNumberSetter), value);
                else
                    this.ring_textbox.Text = value;
            }
        }

        private delegate void AthleteNameDelegate(String value);
        private void AthleteNameSetter(String value) { this.AthleteName = value; }
        public String AthleteName
        {
            get
            {
                return athlete_name_textbox.Text;
            }
            set
            {
                if (this.athlete_name_textbox.InvokeRequired)
                    this.athlete_name_textbox.Invoke(new AthleteNameDelegate(AthleteNameSetter), value);
                else
                    this.athlete_name_textbox.Text = value;
            }
        }

        private void startServer()
        {
            if (this.server.State == StringServer.States.ONLINE) return;

            start_button.Hide();
            port_textbox.ReadOnly = true;
            tab_control.Show();
            scores_tab_control.Show();

            this.Width = 890;
            this.Height = 590;

            updateJudgeCount();

            server.Start(port: Int32.Parse(port_textbox.Text));
            server.ClientConnected += new PoomsaeBoard.StringServer.ClientConnectedEventHandler((object o, PoomsaeBoard.StringServer.ClientConnectedEventArgs a) =>
            {
                TcpClient client = a.Client;
                MessageService.Start(client, (String[] message) => {
                    if (message.Length == 2 && message[0].Equals("register"))
                    {
                        foreach (Judge judge in judges)
                        {
                            if (judge.Passphrase.Equals(message[1]))
                            {
                                if (!judge.Connected)
                                {
                                    //MessageBox.Show("Client accepted!\n" + "Judging as judge " + judge.id + ".");
                                    judge.Client = client;
                                    return false;
                                }
                                else
                                {
                                    //MessageBox.Show("Client rejected!\n" + "Judge with this passphrase is already registered.");
                                    MessageService.sendMessage(client, "rejected", "Judge with this passphrase is already registered.");
                                    client.Close();
                                    return false;
                                }
                            }
                        }

                        //MessageBox.Show("Client rejected!\n" + "No Judge found with this passphrase.");
                        MessageService.sendMessage(client, "rejected", "No Judge found with this passphrase.");
                        client.Close();
                        return false;
                    }

                    return true;
                });
            });
        }

        private double calculateExpression(String expression, Hashtable values = null)
        {
            if (values == null) values = new Hashtable();

            foreach (Object index in values.Keys)
            {
                String field = index as String;
                String result = String.Format("{0:0.0000}", (double)values[index]);
                expression = expression.Replace(field, result);
            }

            try
            {
                String result = (new DataTable()).Compute(expression, null).ToString();
                return double.Parse(result);
            }
            catch
            {
                return 0.0;
            }
        }

        private void updateRules(Ruleset rules)
        {
            foreach (Judge judge in judges)
                judge.setRules(rules);
        }

        private void updateScore()
        {
            if (judges == null || judges.Count == 0) return;

            Hashtable valuesTotal = new Hashtable();

            double[][] scores = new double[2][];
            for (int poomsae = 1; poomsae <= scores.Length; poomsae++)
            {
                Hashtable valuesPoomsae = new Hashtable();
                double value = 0.0, mint = double.MaxValue, maxt = double.MinValue, minp = double.MaxValue, maxp = double.MinValue, sumt = 0.0, sump = 0.0;
                foreach (Judge judge in judges)
                {
                    value = 0.0;
                    double.TryParse(poomsae == 1 ? judge.Technical1 : judge.Technical2, out value);
                    mint = Math.Min(mint, value);
                    maxt = Math.Max(maxt, value);
                    sumt += value;
                    valuesPoomsae.Add("j" + judge.Id + "t", value);

                    value = 0.0;
                    double.TryParse(poomsae == 1 ? judge.Presentation1 : judge.Presentation2, out value);
                    minp = Math.Min(minp, value);
                    maxp = Math.Max(maxp, value);
                    sump += value;
                    valuesPoomsae.Add("j" + judge.Id + "p", value);
                }

                valuesPoomsae.Add("j#", (double)judges.Count);
                valuesPoomsae.Add("mint", mint);
                valuesPoomsae.Add("maxt", maxt);
                valuesPoomsae.Add("avgt", sumt / (double)judges.Count);
                valuesPoomsae.Add("sumt", sumt);
                valuesPoomsae.Add("minp", minp);
                valuesPoomsae.Add("maxp", maxp);
                valuesPoomsae.Add("avgp", sump / (double)judges.Count);
                valuesPoomsae.Add("sump", sump);

                scores[poomsae - 1] = new double[3];

                scores[poomsae - 1][0] = calculateExpression(rule_technical.Text, valuesPoomsae);
                scores[poomsae - 1][1] = calculateExpression(rule_presentation.Text, valuesPoomsae);

                valuesPoomsae.Add("scoret", scores[poomsae - 1][0]);
                valuesPoomsae.Add("scorep", scores[poomsae - 1][1]);

                scores[poomsae - 1][2] = calculateExpression(rule_poomsae.Text, valuesPoomsae);

                valuesTotal.Add("score" + poomsae, scores[poomsae - 1][2]);
            }

            valuesTotal.Add("j#", (double)judges.Count);
            double finalScore = calculateExpression(rule_final.Text, valuesTotal);

            this.Poomsae1.Technical = scores[0][0].ToString();
            this.Poomsae1.Presentation = scores[0][1].ToString();
            this.Poomsae1.Score = scores[0][2].ToString();

            this.Poomsae2.Technical = scores[1][0].ToString();
            this.Poomsae2.Presentation = scores[1][1].ToString();
            this.Poomsae2.Score = scores[1][2].ToString();

            this.Score = finalScore.ToString();

            foreach (Display window in windows)
                window.redraw();
        }

        private void updateJudgeCount()
        {
            int count = 0;
            if (!Int32.TryParse(judgecount_textbox.Text, out count)) return;
            if (count < judges.Count) // Remove Some Judges
            {
                for (int i = judges.Count - 1; i >= count; i--)
                {
                    judge_table.Rows.RemoveAt(i);
                    judges.RemoveAt(i);
                }
                updateScore();
            }
            else if (count > judges.Count) // Add Some Judges
            {
                for (int i = judges.Count + 1; i <= count; i++)
                {
                    judges.Add(new Judge(
                        this,
                        judge_table.Rows[judge_table.Rows.Add(i, "Offline", "0.0", "0.0", "0.0", "0.0", "Judge" + i)],
                        i
                    ));
                }
                updateScore();
            }
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            host_textbox.Text = Dns.GetHostName();
            ip_textbox.Text = GetIPAddress().ToString();
            port_textbox.Text = "3016";

            getRuleSets();
            rulesets_dropdown.SelectedIndex = rulesets_dropdown.Items.Count - 1;
        }

        private void onFormClose(object sender, FormClosedEventArgs e)
        {
            try
            {
                Environment.Exit(0);
                Application.Exit();
            }
            catch { }
        }

        private void onStartClick(object sender, EventArgs e)
        {
            startServer();
        }

        private void onJudgeNumberChange(object sender, EventArgs e)
        {
            updateJudgeCount();
        }

        private void onJudgeNumberValidate(object sender, CancelEventArgs e)
        {
            int result;
            if (!Int32.TryParse(judgecount_textbox.Text, out result)) e.Cancel = true;
        }

        private void createDisplayWindow()
        {
            Display window = new Display(this);
            window.FormClosed += new FormClosedEventHandler((object o, FormClosedEventArgs a) => { windows.Remove((Display)o); return; });
            window.Show(this);
            this.windows.Add(window);
            this.updateScore();
        }

        private void onDisplayClick(object sender, EventArgs e)
        {
            this.createDisplayWindow();
        }

        private void onNameChange(object sender, EventArgs e)
        {
            updateScore();
            foreach (Judge judge in judges)
                MessageService.sendMessage(judge.Client, "name", athlete_name_textbox.Text);
        }

        private void onRingChange(object sender, EventArgs e)
        {
            foreach (Judge judge in judges)
                MessageService.sendMessage(judge.Client, "ring", ring_textbox.Text);
        }

        private void onScoreChange(object sender, EventArgs e)
        {
            updateScore();
        }

        private void onJudgeTableCellEnter(object sender, DataGridViewCellEventArgs e)
        {
            judge_table.BeginEdit(true);
        }

        private void onJudgeTableCellChanged(object sender, DataGridViewCellEventArgs e)
        {
            updateScore();
        }

        private void onSave(object sender, EventArgs e)
        {
            saveFile();
        }

        private void clearScores()
        {
            this.PoomsaeNumber = 1;
            this.AthleteName = "Name";

            this.poomsae1.Technical = "5.0";
            this.poomsae1.Presentation = "5.0";
            this.poomsae1.Score = "10.0";
            this.poomsae1.Time = "0.0";

            this.poomsae2.Technical = "5.0";
            this.poomsae2.Presentation = "5.0";
            this.poomsae2.Score = "10.0";
            this.poomsae2.Time = "0.0";

            foreach (Judge judge in judges)
            {
                judge.Technical1 = "0.0";
                judge.Presentation1 = "0.0";
                judge.Technical2 = "0.0";
                judge.Presentation2 = "0.0";
                MessageService.sendMessage(judge.Client, "clear");
            }

            updateScore();
        }

        private void saveFile()
        {
            // Save Scores

            Record record = new Record();

            record.name = athlete_name_textbox.Text.Replace(",","");    

            if (!Int32.TryParse(judgecount_textbox.Text, out record.judgeCount))
            {MessageBox.Show("Couldn't save record!\nInvalid number of judges!"); return;}

            if (!Double.TryParse(score1_time.Text, out record.time1))
            {MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 time!"); return;}

            if (!Double.TryParse(score1_technical.Text, out record.score1t))
            {MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 technical score!"); return;}

            if (!Double.TryParse(score1_presentation.Text, out record.score1p))
            {MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 presentation score!"); return;}

            if (!Double.TryParse(score1_total.Text, out record.score1f))
            {MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 final score!"); return;}

            if (!Double.TryParse(score2_time.Text, out record.time2))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 time!"); return; }

            if (!Double.TryParse(score2_technical.Text, out record.score2t))
            {MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 technical score!"); return;}

            if (!Double.TryParse(score2_presentation.Text, out record.score2p))
            {MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 presentation score!"); return;}

            if (!Double.TryParse(score2_total.Text, out record.score2f))
            {MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 final score!"); return;}

            if (!Double.TryParse(score_final.Text, out record.scoref))
            { MessageBox.Show("Couldn't save record!\nInvalid combine Poomsae score!"); return; }

            record.judgeScores = new QuadDouble[record.judgeCount];

            foreach (Judge judge in judges)
            {
                record.judgeScores[judge.Id - 1] = new QuadDouble();
                record.judgeScores[judge.Id - 1].score1t = Double.Parse(judge.Technical1);
                record.judgeScores[judge.Id - 1].score1p = Double.Parse(judge.Presentation1);
                record.judgeScores[judge.Id - 1].score2t = Double.Parse(judge.Technical2);
                record.judgeScores[judge.Id - 1].score2p = Double.Parse(judge.Presentation2);
            }

            Directory.CreateDirectory("results");
            String file = "results/" + division_textbox.Text + ".csv";
            Record.append(file, record);

            clearScores();
            loadFile();
        }

        private void loadRecord(Record record)
        {
            athlete_name_textbox.Text = record.name;

            score1_time.Text = record.time1.ToString();
            score1_technical.Text = record.score1t.ToString();
            score1_presentation.Text = record.score1p.ToString();
            score1_total.Text = record.score1f.ToString();

            score2_time.Text = record.time2.ToString();
            score2_technical.Text = record.score2t.ToString();
            score2_presentation.Text = record.score2p.ToString();
            score2_total.Text = record.score2f.ToString();

            score_final.Text = record.scoref.ToString();

            foreach (Judge judge in judges)
            {
                judge.Technical1 = record.judgeScores[judge.Id - 1].score1t.ToString();
                judge.Presentation1 = record.judgeScores[judge.Id - 1].score1p.ToString();
                judge.Technical2 = record.judgeScores[judge.Id - 1].score2t.ToString();
                judge.Presentation2 = record.judgeScores[judge.Id - 1].score2p.ToString();
            }
        }

        private void onEditAthlete(object sender, EventArgs e)
        {
            if (division_table.SelectedCells.Count == 0)
            {
                return;
            }

            DataGridViewRow row = division_table.Rows[division_table.SelectedCells[0].RowIndex];
            Record record = new Record();

            record.name = row.Cells[0].Value.ToString();

            if (!Double.TryParse(row.Cells[1].Value.ToString(), out record.scoref))
            { MessageBox.Show("Couldn't save record!\nInvalid combine Poomsae score!"); return; }

            if (!Double.TryParse(row.Cells[2].Value.ToString(), out record.score1f))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 final score!"); return; }

            if (!Double.TryParse(row.Cells[3].Value.ToString(), out record.score1t))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 technical score!"); return; }

            if (!Double.TryParse(row.Cells[4].Value.ToString(), out record.score1p))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 presentation score!"); return; }

            if (!Double.TryParse(row.Cells[5].Value.ToString(), out record.time1))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 1 time!"); return; }

            if (!Double.TryParse(row.Cells[6].Value.ToString(), out record.score2f))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 final score!"); return; }

            if (!Double.TryParse(row.Cells[7].Value.ToString(), out record.score2t))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 technical score!"); return; }

            if (!Double.TryParse(row.Cells[8].Value.ToString(), out record.score2p))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 presentation score!"); return; }

            if (!Double.TryParse(row.Cells[9].Value.ToString(), out record.time2))
            { MessageBox.Show("Couldn't save record!\nInvalid Poomsae 2 time!"); return; }

            record.judgeCount = judges.Count;
            record.judgeScores = new QuadDouble[record.judgeCount];

            foreach (Judge judge in judges)
            {
                var i = judge.Id - 1;
                record.judgeScores[judge.Id - 1] = new QuadDouble();
                record.judgeScores[judge.Id - 1].score1t = Double.Parse(row.Cells[10 + i * 4].Value.ToString());
                record.judgeScores[judge.Id - 1].score1p = Double.Parse(row.Cells[11 + i * 4].Value.ToString());
                record.judgeScores[judge.Id - 1].score2t = Double.Parse(row.Cells[12 + i * 4].Value.ToString());
                record.judgeScores[judge.Id - 1].score2p = Double.Parse(row.Cells[13 + i * 4].Value.ToString());
            }

            this.loadRecord(record);
        }

        private void onDivisionChange(object sender, EventArgs e)
        {
            loadFile();
        }

        private void loadFile()
        {
            division_table.Rows.Clear();

            Directory.CreateDirectory("results");
            String file = "results/" + division_textbox.Text + ".csv";
            Record[] records = Record.read(file);

            if (records == null || records.Length == 0) return;

            foreach (Record record in Record.read(file))
            {
                DataGridViewRow row = division_table.Rows[division_table.Rows.Add(
                    record.name,
                    record.scoref,
                    record.score1f,
                    record.score1t,
                    record.score1p,
                    record.time1,
                    record.score2f,
                    record.score2t,
                    record.score2p,
                    record.time2
                )];

                int judgeColumns = (division_table.Columns.Count - 10) / 4;
                for (int i = judgeColumns + 1; i <= record.judgeCount; i++)
                {
                    division_table.Columns.Add("j" + i + "t1column", "J" + i + "T1");
                    division_table.Columns.Add("j" + i + "p1column", "J" + i + "P1");
                    division_table.Columns.Add("j" + i + "t2column", "J" + i + "T2");
                    division_table.Columns.Add("j" + i + "p2column", "J" + i + "P2");
                }

                for (int i = 0; i < record.judgeCount; i ++)
                {
                    row.Cells[10 + i * 4].Value = record.judgeScores[i].score1t;
                    row.Cells[11 + i * 4].Value = record.judgeScores[i].score1p;
                    row.Cells[12 + i * 4].Value = record.judgeScores[i].score2t;
                    row.Cells[13 + i * 4].Value = record.judgeScores[i].score2p;
                }
            }
        }

        private void onRuleSetChanged(object sender, EventArgs e)
        {
            updateScore();
        }

        private void onSaveRuleSet(object sender, EventArgs e)
        {
            saveRuleSet();
        }
        
        private void onLoadRuleSet(object sender, EventArgs e)
        {
            loadRuleSet();
        }

        private void onShowRuleSets(object sender, EventArgs e)
        {
            getRuleSets();
        }

        private void getRuleSets()
        {
            Directory.CreateDirectory("rulesets");
            String[] files = Directory.GetFiles("rulesets");

            rulesets_dropdown.Items.Clear();
            foreach (String file in files)
            {
                int start = file.IndexOf("rulesets") + 9;
                int end = file.IndexOf(".csv") - 1;
                rulesets_dropdown.Items.Add(file.Substring(start, end - start + 1));
            }
        }

        public Ruleset getRuleSet()
        {
            Ruleset rules = new Ruleset();

            rules.technical = rule_technical.Text;
            rules.technicalMinor = rule_minor.Text;
            rules.technicalMajor = rule_major.Text;
            rules.technicalMin = rule_min.Text;
            rules.technicalMax = rule_max.Text;

            rules.presentation = rule_presentation.Text;
            int length = rule_presentations.Rows.Count - 1;
            if (length >= 0)
                rules.presentations = new Ruleset.PresentationRule[length];
            for (int i = 0; i < length; i++)
            {
                rules.presentations[i] = new Ruleset.PresentationRule();

                if (rule_presentations.Rows[i].Cells[0].Value != null)
                    rules.presentations[i].name = rule_presentations.Rows[i].Cells[0].Value.ToString();

                if (rule_presentations.Rows[i].Cells[1].Value != null)
                    rules.presentations[i].min = rule_presentations.Rows[i].Cells[1].Value.ToString();

                if (rule_presentations.Rows[i].Cells[2].Value != null)
                    rules.presentations[i].max = rule_presentations.Rows[i].Cells[2].Value.ToString();

                if (rule_presentations.Rows[i].Cells[3].Value != null)
                    rules.presentations[i].step = rule_presentations.Rows[i].Cells[3].Value.ToString();
            }

            rules.poomsae = rule_poomsae.Text;
            rules.final = rule_final.Text;

            return rules;
        }

        private void loadRuleSet()
        {
            String name = rulesets_dropdown.Text;
            Directory.CreateDirectory("rulesets");
            String file = "rulesets/" + name + ".csv";
            Ruleset[] rules = Ruleset.read(file);

            if (rules == null || rules.Length == 0)
            {
                MessageBox.Show("Rule set \"" + name + "\" was not found!");
                return;
            }

            rule_technical.Text = rules[0].technical;
            rule_minor.Text = rules[0].technicalMinor;
            rule_major.Text = rules[0].technicalMajor;
            rule_min.Text = rules[0].technicalMin;
            rule_max.Text = rules[0].technicalMax;

            rule_presentation.Text = rules[0].presentation;
            rule_presentations.Rows.Clear();
            for (int i = 0; i < rules[0].presentations.Length; i ++ )
            {
                rule_presentations.Rows.Add(
                    rules[0].presentations[i].name,
                    rules[0].presentations[i].min,
                    rules[0].presentations[i].max,
                    rules[0].presentations[i].step
                );
            }

            rule_poomsae.Text = rules[0].poomsae;
            rule_final.Text = rules[0].final;

            updateRules(rules[0]);
            updateScore();
        }

        private void saveRuleSet()
        {
            String name = rulesets_dropdown.Text;
            Directory.CreateDirectory("rulesets");
            String file = "rulesets/" + name + ".csv";
            Ruleset.write(file, getRuleSet());
            rulesets_dropdown.Items.Add(name);
        }

        private void onDivisionClear(object sender, EventArgs e)
        {
            division_table.Rows.Clear();
            Directory.CreateDirectory("results");
            String file = "results/" + division_textbox.Text + ".csv";
            File.Delete(file);
        }

        private void onDivisionOpen(object sender, EventArgs e)
        {
            Directory.CreateDirectory("results");
            System.Diagnostics.Process folderProcess = new System.Diagnostics.Process();
            folderProcess.StartInfo.FileName = "results";
            folderProcess.Start();
        }

        private void onDivisionReload(object sender, EventArgs e)
        {
            loadFile();
        }

        private System.Timers.Timer timer = null;
        private DateTime startTime;

        private void toggleTimer()
        {
            if (timer == null)
            {
                this.startTime = DateTime.Now;
                this.Poomsae.Time = "0.0";

                this.Mode = Display.Modes.TIMER;
                this.timer_button.Text = "Stop Timer\nCTRL-T";

                this.timer = new System.Timers.Timer(50);
                this.timer.Elapsed += new ElapsedEventHandler(this.onTimer);
                this.timer.Enabled = true;
            }
            else
            {
                this.timer.Enabled = false;
                this.timer.Dispose();
                this.timer = null;

                this.Poomsae.Time = (DateTime.Now.Subtract(startTime)).TotalSeconds.ToString();

                this.Mode = this.PoomsaeMode;
                this.timer_button.Text = "Start Timer\nCTRL-T";
            }
        }

        private void toggleTimer(object sender, EventArgs e)
        {
            this.toggleTimer();
        }

        private void onTimer(object o, ElapsedEventArgs a)
        {
            if (this.timer == null) return;

            this.Poomsae.Time = (a.SignalTime.Subtract(startTime)).TotalSeconds.ToString();

            foreach (Display window in windows)
                window.redraw();
        }

        private delegate void setTextDelegate(TextBox textBox, String text);
        private void setText(TextBox textBox, String text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new setTextDelegate(setText), new Object[] { textBox, text });
                return;
            }

            textBox.Text = text;
        }

        private void onTimeChanged(object sender, EventArgs e)
        {
            updateScore();
        }

        private void onPoomsaeChanged(object sender, EventArgs e)
        {
            if (this.sports_radio.Checked) this.Mode = Display.Modes.SPORTS_POOMSAE;
            if (this.regular_radio.Checked) this.Mode = Display.Modes.REGULAR_POOMSAE;
        }

        private void onPoomsae1NumberChanged(object sender, EventArgs e)
        {
            if (this.poomsae1_radio.Checked) this.PoomsaeNumber = -1;
        }

        private void onPoomsae2NumberChanged(object sender, EventArgs e)
        {
            if (this.poomsae2_radio.Checked) this.PoomsaeNumber = -2;
        }

        private void onRulesChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.updateRules(this.getRuleSet());
        }

        private void onRulesChanged(object sender, EventArgs e)
        {
            this.updateRules(this.getRuleSet());
        }

        private void onRulesChanged(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.updateRules(this.getRuleSet());
        }

        private void onRulesChanged(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            this.updateRules(this.getRuleSet());
        }

        private void onKeyPress(object sender, KeyPressEventArgs e)
        {
            // MessageBox.Show("Test: " + (int)e.KeyChar + ", " + (int)Control.ModifierKeys);

            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyChar == 19)
                {
                    this.saveFile();
                    e.Handled = true;
                }
                else if (e.KeyChar == 14)
                {
                    this.clearScores();
                    e.Handled = true;
                }
                else if (e.KeyChar == 23)
                {
                    this.createDisplayWindow();
                    e.Handled = true;
                }
                else if (e.KeyChar == 20)
                {
                    this.toggleTimer();
                    e.Handled = true;
                }
            }
        }
    }
}
