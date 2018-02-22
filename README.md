# fangraph-priceguide-generator
A command line tool to take exported fangraph projections and convert them to a format that will work with the (Priceguide)[https://github.com/mayscopeland/priceguide] application.


## Why
> The Price Guide generates fantasy baseball dollar values customized for a myriad of league configurations. Dollar values can be customized for the number of teams, number of starters at each position, stat categories used, etc.

Priceguide is flexible and allows you to use a variety of data sources (Zips, Zeile, etc.) as long as the source adheres to a fixed format for hitters and pitchers. To achieve this, Priceguide uses .csv files as it's datasource. 

The challenge from year to year is generating a new set of stats from the different sources.

Fangraphs is a great resource for projections. It currently offers five projections that are free to download.

The Frangraph format does not match the format needed by Priceguide and needs to be supplemented with data from other sources. 

This project was created to automate this process of using Fangraphs and other sources to generate the Priceguide files. 


## Data Sources
A list of the datasources used to generate the Priceguide files:

### Fantasy Baseball ID Mapping
This file provides a "universal" mapping of player ids. It will link a players "mlb_id" to id's for CBS, ESPN, Fangraphs, and more. This translation is useful when mapping against sources and is used by the Greasemonkey script that dipslay's player values while looking at your team.

* http://crunchtimebaseball.com/baseball_map.html


### Lahman Statistical Archives
A collection of all historical baseball statistics from every previous year. For our needs this source provides us with a list of "appearances" by position for previous years.

* http://www.seanlahman.com/baseball-archive/statistics/


### Fangraphs
A collection of baseball projections that are freely available for export.  

* https://www.fangraphs.com/

*Note: Go to Fangraphs > Projections (menu item) > {choose a projection} > right above the data table on the righ hand side is the "Export Data" option.*

