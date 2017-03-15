# Solr Analyzer for Sitecore
Tested on Sitecore 8.2 update 2 but should work on previous verions.  Has ability to work for Solr 4, 5, and 6.

## What is this?
This tool will analyze the performance of the queries that have been running against Solr. Each query is an HTTP call and has a consequence in downloading the requested data. Try to tune your queries to be lean, this will help in downloading a smaller amount of data while applying the same intended value. You will also save on bandwidth between the server and the requestor.

![alt text](https://raw.githubusercontent.com/tbraga/SolrAnalyzer/master/screenshots/query-analyzer.png "Query Analyzer")

## Performance tips to keep in mind
* Select only the fields on the document you need for your scenario. If you only need 10 of the 50 fields for a specific query only call those 10, this will reduce the amount of bytes during the response download.
* Make sure you are only requesting the number of documents you need by using the ROWS parameter within your request. If you need to paginate you can use the ROWS parameter in combination with the START parameter.
* Caching: Optimize the document cache on each of your heavily utilized cores. Specifically look at the eviction rate and the hit ratio.
* By running the query through the Chrome browser, you can look at the network tab of Chrome's Developer Tools and see the byte size returned from Solr as well as the response time. Getting these two numbers to perform are key to a well optimized environment.

## Switching Solr Versions
If you are using Solr 4 or 5 you'll need to modify the SolrAnalyzer.config to enable the proper service.  Solr 6 is enabled by default.  The difference between these is simply how it reads the Solr log looking for queries that have been run.  You'll want to build the project, setup a publishing target in Visual Studio to push the files into a Sitecore instance.  
```xml
<settings>
  <setting name="SolrAnalyzer.LogFile.Location" value="C:\solr-6.4.1\server\logs\solr.log"/>
</settings>
```

## Setup
.  From here you'll want to build the project, setup a publishing target in Visual Studio to push the files into a Sitecore instance.  Make sure Solr is 
outputting queries to its log file.  

In addition, the `ItemData` attribute can be used to set properties on the item:


## How do I get started?

  1. Clone the project from Github.
  2. Update the packages installed from NPM
  3. Update the configuration file to point to your Solr log file (not a Sitecore log file):
      * Config Location: /App_Config/Include/SolrAnalyzer/SolrAnalyzer.config
  4. Within the configuration file, verify the appropriate service class is enabled.  See the above section on Switching Solr Versions.  
  5. Build project
  6. Set a publishing target within Visual Studio and publish the project over to your Sitecore website. 
  7. Install Sitecore package which adds a link to the Sitecore control panel to launch the tool.

```xml
<services>
  <configurator type="Sitecore.SharedSource.SolrAnalyzer.Configurator.SolrAnalyzerConfigurator6, Sitecore.SharedSource.SolrAnalyzer"/>
</services>
```

## Loading the Analyzer
From the Sitecore Control Panel, after you have gone through the steps above and have installed the Sitecore Package.  You will find a link under the Indexing section called Query Analyzer.  Give that a click! :)