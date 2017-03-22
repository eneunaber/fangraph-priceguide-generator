
namespace fangraph_priceguide_generator
{
    public class FangraphPitchingRecord
    {
        public string playerName {get;set;}
        public string team	{get;set;}
        public string league	{get;set;}
        public int G {get;set;} 
        public int GS {get;set;} 
        public int	W {get;set;}
        public int L {get;set;}
        public int S {get;set;}
        public int 	BS {get;set;}
        public int	HLD {get;set;}
        public int 	CG {get;set;}
        public int SHO {get;set;}
        public double 	IP {get;set;}
        public int 	H {get;set;}
        public int 	R {get;set;}
        public int ER {get;set;}	
        public int HR {get;set;}
        public int BB {get;set;}
        public int K {get;set;}
        public int? mlbamID {get;set;}
        public string playerid {get;set;}
        public int G_SP {get;set;}
        public int G_RP {get;set;} 
        public string defaultPos {get;set;}
    }
}

//Name	Team	W	L	[ERA]	GS	G	IP	H	ER	HR	SO	BB	WHIP	K/9	BB/9	FIP	WAR	ADP	playerid