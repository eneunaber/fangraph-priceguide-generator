# fangraph-priceguide-generator
A command line tool to take exported fangraph projections and convert them to a format that will work with the (Priceguide)[https://github.com/mayscopeland/priceguide] application.

## Why
> The Price Guide generates fantasy baseball dollar values customized for a myriad of league configurations. Dollar values can be customized for the number of teams, number of starters at each position, stat categories used, etc.

Priceguide is flexible and allows you to use a variety of data sources (Zips, Zeile, etc.) as long as the source adheres to a fixed format for hitters and pitchers. To achieve this, Priceguide uses .csv files as it's datasource. 

The challenge from year to year is generating a new set of stats from the different sources.

Fangraphs is a great resource for projections. It currently offers five projections that are free to download.

The Frangraph format does not match the format needed by Priceguide and needs to be supplemented with data from other sources. 

This project was created to automate this process of using Fangraphs and other sources to generate the Priceguide files. 

## The Application
This application has two commands that generate different types of file(s). Those commands are *conversion-record* and *convert-fangraph*. 

### conversion-record
The conversion-record command creates an IdConversion-FULL.csv file. This is a mapping file that links a players "mlbamId" to the player id for variou fantasy websites (espn, yaoo, cbs, etc.). Priceguide has a Greasemonkey script that allows for prices to be displayed inside the UI of your fantasy website. The IdConversion file is used as part of this.

This command takes in two parameters.
1. Location of the master.csv (from data source 'Map of MLB Player Names and IDs').
1. The location the file is to be saved to.

#### How it works
The master.csv file is read and converted to a list of in-memory records. That list is converted to the format that is used by Priceguid and then saved.

#### How to run
priceguide-generator conversion-record -m=<location of master.csv> -s=<output directory>

**Note: '-New' will be added to the file name, so the final file will be called 'IdConverstion-FULL-New.csv'

### convert-fangraph
This command will combine data from the master, appearances, and the fangraph file to create a record for the Priceguied application.

This command takes in two parameters.
1. The previous mlb year (e.g. 2016)
1. Location of the master.csv (from data source 'Map of MLB Player Names and IDs').
1. Location of the Appearances.csv (from data source 'Lahman Statistical Archives').
1. The location the file(s) is to be saved to.
1. A variable length arguement, that will take in any number of file locations to FanGraph projection files.

#### How it works
The master.csv and appearances files are read and converted to a list of in-memory records. Then the same is done for all of the Fangraph files. For each Fangraph file, either a Batting or Pitching conversion takes place. 

A player's position, team, mlbamid is pulled from the master.csv.
A player's games played per position is filled in using the previous years values. Which is found inside the Appearances.csv. 
The rest of the fields are converted from the Fangraph file.

#### How to run
priceguide-generator convert-fangraph 
                -py=<previous year [2016]> 
                -m=<location of master.csv>
                -l=<location of Lahman Appearances.csv>
                -s=<output directory>
                -f=<location of fangraphs projection file>
                -f=<location of another fangraphs projection file>
                -f=<...>

## Data Sources
A list of the datasources used to generate the Priceguide files:

### Map of MLB Player Names and IDs
This file provides a "universal" mapping of player ids. It will link a players "mlb_id" to id's for CBS, ESPN, Fangraphs, and more. This translation is useful when mapping against sources and is used by the Greasemonkey script that dipslay's player values while looking at your team.

* http://crunchtimebaseball.com/baseball_map.html


### Lahman Statistical Archives
A collection of all historical baseball statistics from every previous year. For our needs this source provides us with a list of "appearances" by position for previous years.

* http://www.seanlahman.com/baseball-archive/statistics/
* https://github.com/chadwickbureau/baseballdatabank (version hosted on GitHub)


### Fangraphs
A collection of baseball projections that are freely available for export.  

* https://www.fangraphs.com/

*Note: Go to Fangraphs > Projections (menu item) > {choose a projection} > right above the data table on the righ hand side is the "Export Data" option.*