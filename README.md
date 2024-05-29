# INTRODUCTION

Welcome to the DigiInsight __smartcache_samples__ repository. <br>
This repository contains sample code and examples to help you understand and effectively use the [__DigiInsight SmartCache__](https://github.com/diginsight/smartcache). 

The samples cover a wide range of use cases and demonstrate the capabilities and features of the SmartCache. Whether you're a beginner or an experienced developer, these samples will provide valuable insights into the workings of DigiInsight SmartCache.

# GETTING STARTED

## Step 01: clone the repo and open SmartCache.Samples.sln solution
![alt text](<docs/001.03 - smartcache_samples repo.png>)

you can use the following solutions:
- __SmartCache.Samples.sln__ to run the samples using diginsight as a package reference.
- __SmartCache.Samples.Debug.sln__ to run the samples using diginsight code as a project reference

## Step 02: run SampleWebApi and call getplants operation

After running  the sample you'll obtain se __service swagger__ shown: 
![alt text](<docs/002.01a - service started.png>)

After calling the operation your `%userprofile%\LogFiles\Diginsight` folder will contain the following __log file__:
![alt text](<docs/002.02b - cache miss log.png>)

Please note the call has a __cache miss__ from `PlantsController.GetPlantsAsync` and latency is __over 1 second__ when obtaining data from ``PlantsController.GetPlantsImplAsync``.

## Step 02:call getplants operation a second time
The second time you call `PlantsController.GetPlantsAsync` you'll get a __cache hit__ and latency will be __5ms__.

![alt text](<docs/003.01 - cache miss log.png>)


# Additional information

Additional information is available in the following articles:<br>

>- [Introduction to diginsight smartcache](https://github.com/diginsight/smartcache?tab=readme-ov-file#introduction)<br>
>Explores __basic concepts about diginsight smartcache__ and how to integrate it<br>
>- [HowTo: Debug samples using diginsight project references](https://github.com/diginsight/telemetry?tab=readme-ov-file#GETTING-STARTED)<br>
>Explores how to use the samples solution and __debug__ diginsight code<br>



