namespace fangraph_priceguide_generator.Models
{
    public class FangraphHitterRecord
    {
        public string playerName {get;set;}
        public string team	{get;set;}
        public string league	{get;set;}
        public int G {get;set;} 
        public int	AB {get;set;}
        public int H {get;set;}
        public int Doubles {get;set;}
        public int 	Triples {get;set;}
        public int	HR {get;set;}
        public int 	R {get;set;}
        public int RBI {get;set;}
        public int 	BB {get;set;}
        public int 	SO {get;set;}
        public int 	HBP {get;set;}
        public int SB {get;set;}	
        public int CS {get;set;}
        public int? mlbamID {get;set;}
        public string playerid {get;set;}
        public int G_C {get;set;}
        public int G_1B {get;set;}
        public int G_2B {get;set;}
        public int G_3B {get;set;}
        public int G_SS {get;set;}
        public int G_LF {get;set;}
        public int G_CF {get;set;}
        public int G_RF {get;set;}
        public int G_OF {get;set;}     
        public string defaultPos {get;set;}
    }
}