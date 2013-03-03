using System;
using System.IO;
using Gtk;
using System.Xml;
using System.Collections.Generic;


public partial class MainWindow: Gtk.Window
{	
	
	public XmlDocument xDoc = new XmlDocument();


	public int randomBox;
	public List<Gtk.TextView> boxes = new List<Gtk.TextView>();
	public List<int> randomIndexes = new List<int>();
	public Random rand = new Random();
	public int corrects = 0;
	public int falses = 0;
	public int left = 0;
	public int numVal;
	public string base_directory = System.AppDomain.CurrentDomain.BaseDirectory;
	
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		string facts_xml = base_directory + "facts.xml";
		xDoc.Load(facts_xml);
	}
	
	
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void onExit (object sender, System.EventArgs e)
	{
		Application.Quit ();
	}

	protected void onAbout (object sender, System.EventArgs e)
	{
		AboutDialog about = new AboutDialog();
     
     // Change the Dialog's properties to the appropriate values.
     	string[] authors = new string[1] {"Σαββόπουλος Κώστας"};
		about.ProgramName = "Βυζαντινή Ιστορία - The Game!";
		about.Website = "http://www.pansoft.gr";
    	about.Authors = authors;
		about.Version = "0.0.01";
     
     // Show the Dialog and pass it control
     about.Run();
     
     // Destroy the dialog
     about.Destroy();
	}
	
	protected void onStartClicked (object sender, System.EventArgs e)
	{
		XmlTextReader reader = new XmlTextReader ("facts.xml");
			while (!reader.EOF && reader.Name != "year"){
				reader.Read ();
			}
			label2.LabelProp = "<span font-size = 'x-large'>"+ reader.GetAttribute("id") + "</span>";
			reader.Read (); // Move from "item" to "title"
			while (reader.NodeType == XmlNodeType.Whitespace){
				reader.Read ();
			}
			textview1.Buffer.Text = reader.ReadString ();
		reader.Close();
	}

	protected void OnSearchClicked (object sender, System.EventArgs e)
	{
		foreach(XmlNode node in xDoc.GetElementsByTagName("year"))
		{
			if (node.Attributes["id"].Value == entry2.Text)
			{
				label2.LabelProp = "<span font-size = 'x-large'>" + node.Attributes["id"].Value + "</span>";
				textview1.Buffer.Text="";
				foreach(XmlNode child in node.ChildNodes)
				{
					textview1.Buffer.Text+=child.InnerText + "\n\n";
				}
			}
		}
		
	}

	protected void OnNextClicked (object sender, System.EventArgs e)
	{
		int i=0;
		int index = 600;
		foreach(XmlNode node in xDoc.GetElementsByTagName("year"))
		{
			if(label2.Text == node.Attributes["id"].Value && index == 600){
				if (i == xDoc.GetElementsByTagName("year").Count - 1)
				{
					index = 0;
				}
				else
				{
					index = i+1;	
				}
				label2.LabelProp = "<span font-size = 'x-large'>" + xDoc.GetElementsByTagName("year").Item(index).Attributes["id"].Value +"</span>";
				textview1.Buffer.Text = "";
				foreach(XmlNode child in xDoc.GetElementsByTagName("year").Item(index))
				{
					textview1.Buffer.Text+=child.InnerText + "\n\n";	
				}
			}
			i++;
		}
	}

	protected void OnPrevClicked (object sender, System.EventArgs e)
	{
		int i=0;
		int index = 600;
		foreach(XmlNode node in xDoc.GetElementsByTagName("year"))
		{
			if(label2.Text == node.Attributes["id"].Value && index == 600){
				if (i == 0){
					index = xDoc.GetElementsByTagName("year").Count - 1;
				}
				else{
					index = i-1;	
				}
				label2.LabelProp = "<span font-size = 'x-large'>" + xDoc.GetElementsByTagName("year").Item(index).Attributes["id"].Value + "</span>";
				textview1.Buffer.Text = "";
				foreach(XmlNode child in xDoc.GetElementsByTagName("year").Item(index))
				{
					textview1.Buffer.Text+=child.InnerText + "\n\n";	
				}
				
			}
			i++;
		}
	}

	protected void OnSubmitClicked (object sender, System.EventArgs e)
	{
		
		
		numVal = -1;
		int qNumber = -1;
		int qCount = 0;
		try
        {
            numVal = Convert.ToInt32(entry1.Text);
        }
		catch (FormatException ){}
		catch (OverflowException ){}
        finally
        {
            if (numVal <= 500 && numVal>0 )
            {
             	qNumber = numVal;
				while(qCount<qNumber)
				{
					int randomIndex;
					randomIndex = rand.Next(0,xDoc.GetElementsByTagName("year").Count);
					if(!randomIndexes.Contains(randomIndex))
					{
						randomIndexes.Add(randomIndex);
						qCount++;
					}
					
				}
	
				hbox4.Visible = false;
				label5.Visible = true;
				hbox5.Visible = true;
				hbox6.Visible = true;
				hbox7.Visible = true;
				hbox8.Visible = true;
				hbox10.Visible = true;
		
				
				boxes.Add(textview2);
				boxes.Add(textview3);
				boxes.Add(textview4);
				boxes.Add(textview5);
				randomBox = rand.Next(0,boxes.Count);
				int randomChild = rand.Next(0,xDoc.GetElementsByTagName("year")[randomIndexes[0]].ChildNodes.Count);
				foreach(Gtk.TextView box in boxes)
				{
					if (boxes.IndexOf(box)==randomBox)
					{
						
						foreach(XmlNode child in xDoc.GetElementsByTagName("year").Item(randomIndexes[0]).ChildNodes)
						{
							if(child.Equals(xDoc.GetElementsByTagName("year").Item(randomIndexes[0]).ChildNodes.Item(randomChild)))
							{
								label5.Text = "Τι από τα παρακάτω έγινε το ";
								label5.Text += xDoc.GetElementsByTagName("year").Item(randomIndexes[0]).Attributes[0].Value;
								label5.Text +=" ?";
								box.Buffer.Text = child.InnerText;
							}
						}
					}
					else
					{
						int randomYear =rand.Next(0,xDoc.GetElementsByTagName("year").Count);
						int randomFact = rand.Next(0,xDoc.GetElementsByTagName("year")[randomYear].ChildNodes.Count);
						box.Buffer.Text = xDoc.GetElementsByTagName("year")[randomYear].ChildNodes[randomFact].InnerText;
					}
				}
			
				randomIndexes.RemoveAt(0);
			
				
				
            }
        }
		
	}


	protected void OnAnswerButtonClicked (object sender, System.EventArgs e)
	{
		button6.Visible=false;
		button7.Visible=true;
		var radios = new List<RadioButton>();
		radios.Add(radiobutton1);
		radios.Add(radiobutton2);
		radios.Add(radiobutton3);
		radios.Add(radiobutton4);
		
		foreach(RadioButton radio in radios)
		{
			if(radio.Active)
			{
				if (radios.IndexOf(radio)==randomBox)
				{
					image1.Visible=true;
					image2.Visible=false;
					corrects++;
				}
				else
				{
					image2.Visible=true;
					image1.Visible=false;
					falses++;
				}
				boxes[randomBox].ModifyBase(StateType.Normal,new Gdk.Color(171,241,244));
			}
			
		
		}
		statusbar1.Pop(1);
		int left = randomIndexes.Count;
		string message = String.Format ("Σωστά :{0}, Λάθος :{1}, Απομένουν :{2}", corrects,falses,left);
		statusbar1.Push(1,message);
	}

	protected void onNextQuestionClicked (object sender, System.EventArgs e)
	{
		boxes[randomBox].ModifyBase(StateType.Normal,new Gdk.Color(255,255,255));
		if(randomIndexes.Count!=0)
		{
			randomBox = rand.Next(0,boxes.Count);
			int randomChild = rand.Next(0,xDoc.GetElementsByTagName("year")[randomIndexes[0]].ChildNodes.Count);
			foreach(Gtk.TextView box in boxes)
				{
					if (boxes.IndexOf(box)==randomBox)
					{
						
						foreach(XmlNode child in xDoc.GetElementsByTagName("year").Item(randomIndexes[0]).ChildNodes)
						{
							if(child.Equals(xDoc.GetElementsByTagName("year").Item(randomIndexes[0]).ChildNodes.Item(randomChild)))
							{
								label5.Text = "Τι από τα παρακάτω έγινε το ";
								label5.Text += xDoc.GetElementsByTagName("year").Item(randomIndexes[0]).Attributes[0].Value;
								label5.Text +=" ?";
								box.Buffer.Text = child.InnerText;
							}
						}
					}
					else
					{
						int randomYear =rand.Next(0,xDoc.GetElementsByTagName("year").Count);
						int randomFact = rand.Next(0,xDoc.GetElementsByTagName("year")[randomYear].ChildNodes.Count);
						box.Buffer.Text = xDoc.GetElementsByTagName("year")[randomYear].ChildNodes[randomFact].InnerText;
					}
				}
			
				randomIndexes.RemoveAt(0);
			button6.Visible=true;
			button7.Visible=false;
		}
		else
		{
			float cor = (float)corrects;

			float total = (float)numVal;
			label6.Text = "Score : " + (cor / total)*100 +"%";
			vbox4.Visible = true;
			label5.Visible = false;
			hbox5.Visible = false;
			hbox6.Visible = false;
			hbox7.Visible = false;
			hbox8.Visible = false;
			hbox10.Visible = false;
			statusbar1.Pop(1);
			
			
		}
	}

	protected void PlayAgainClicked (object sender, System.EventArgs e)
	{
		button6.Visible = true;
		button7.Visible = false;
		hbox4.Visible = true;
		vbox4.Visible = false;
		corrects = 0;
		falses = 0;
		left = 0;
	}
}
