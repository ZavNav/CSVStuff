using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CSVStuff
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            List<Pepka> kek = Refresh(input/*Path.GetFullPath("inputNew.csv")*/);
            List<Pepka> lol = kek.OrderBy(x => x.id).ToList();
            int uniqcounter = 0;
            List<int> ids = new List<int>();
            int temp = -1;
            
            foreach (var item in lol)
            {
                if (item.id != temp)
                {
                    uniqcounter++;
                    ids.Add(item.id);
                }
                temp = item.id;
            }

            List<List<Pepka>> pepos = new List<List<Pepka>>();
            foreach (var item in ids)
            {
                pepos.Add(lol.Where(x => x.id == item).ToList());
            }

            List<List<Pepka>> final = new List<List<Pepka>>();
            foreach (var item in pepos)
            {
                if (item.Any(x => x.prType == "NULL"))
                {
                    int tempid = item[0].id;
                    string tempdate = item[0].dates;
                    int nullindex = -1;

                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item[i].prType == "NULL")
                        {
                            nullindex = i;
                        }
                    }
                    item.RemoveAt(nullindex);
                    
                    for (int i = 0; i < 3; i++)
                    {
                        switch (i)
                        {
                           case 0: item.Insert(i+nullindex,new Pepka{id = tempid, dates = tempdate, prType = "KGT"}); break;
                           case 1: item.Insert(i+nullindex,new Pepka{id = tempid, dates = tempdate, prType = "COLD"}); break;
                           case 2: item.Insert(i+nullindex,new Pepka{id = tempid, dates = tempdate, prType = "OTHER"}); break;
                        }
                    }
                }

                final.Add(item.OrderBy(x => x.prType[1]).ThenBy(x => DateTime.Parse(x.dates.Split(' ')[0])).ToList());

            }

            foreach (var item in final)
            {
                foreach (var j in item)
                {
                    Console.WriteLine($"{j.id}, {j.dates}, {j.prType}");
                }
            }

            string path = input/*"/Users/zavnav/RiderProjects/CSVStuff/CSVStuff/bin/Debug/net5.0/output.csv"*/;
            
            DataTable table = new DataTable();  
            table.Columns.Add();     
            table.Columns.Add();     
            table.Columns.Add();
            foreach (var item in final)
            {
                foreach (var j in item)
                {
                    table.Rows.Add(j.id, j.dates, j.prType);
                }
            } 
            table.ToCSV(path);
        }

        public static List<Pepka> Refresh(string filename)
        {
            List<Pepka> res = new List<Pepka>();
            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Pepka p = new Pepka();
                    p.piece(line);
                    res.Add(p);
                }
            }
 
            return res;
        }
    }
    public static class  CSVUtlity  { 
        public static void ToCSV(this DataTable dtDataTable, string strFilePath)
        { 
            StreamWriter sw = new StreamWriter(strFilePath, false);  
            
            foreach(DataRow dr in dtDataTable.Rows) {  
                for (int i = 0; i < dtDataTable.Columns.Count; i++) {  
                    if (!Convert.IsDBNull(dr[i])) {  
                        string value = dr[i].ToString();  
                        if (value.Contains(',')) {  
                            value = String.Format("\"{0}\"", value);  
                            sw.Write(value);  
                        } else {  
                            sw.Write(dr[i].ToString());  
                        }  
                    }  
                    if (i < dtDataTable.Columns.Count - 1) {  
                        sw.Write(",");  
                    }  
                }  
                sw.Write(sw.NewLine);  
            }  
            sw.Close();  
        } 
    }
    class Pepka
    {
        public int id { get; set; }
        public string dates { get; set; }
        public string prType { get; set; }
        
        public void piece (string line)
        {
            string[] parts = line.Split(',');
            id = int.Parse(parts[0]);
            dates = parts[1];
            prType = parts[2];
        }

        public void DatesSort()
        {
            
        }
    }
}