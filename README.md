# INTRODUCTION

Welcome to the DigiInsight __smartcache_samples__ repository. <br>
This repository contains sample code and examples to help you understand and effectively use the [__DigiInsight SmartCache__](https://github.com/diginsight/smartcache). 

The samples cover a wide range of use cases and demonstrate the capabilities and features of the SmartCache. Whether you're a beginner or an experienced developer, these samples will provide valuable insights into the workings of DigiInsight SmartCache.

# GETTING STARTED

## Step 01: clone the repo and open SmartCache.Samples.sln solution
![alt text](<docs/001.03 - smartcache_samples repo.png>)

you can use the following solutions:
- __SmartCache.Samples.sln__ to run the samples using diginsight as a package reference.
- __SmartCache.Samples.Debug.sln__ to run the samples using diginsight code as a project reference<br>
    > pleae, note that diginsight `telemetry` and `smartcache` repositories must also be cloned on the same root folder as `smartcache_samples` repository.

## Step 02: run SampleWebApi and call getplants operation

After running  the sample you'll obtain se __service swagger__ shown: 
![alt text](<docs/002.01a - service started.png>)

After calling the operation your `%userprofile%\LogFiles\Diginsight` folder will contain the `SampleWebApi` __log file__:
![alt text](<docs/002.01b - SampleWebApi log file.png>)

Please note the call has a __cache miss__ from `PlantsController.GetPlantsAsync` and latency is __over 1 second__ when obtaining data from ``PlantsController.GetPlantsImplAsync``.
![alt text](<docs/002.02b - cache miss log.png>)


## Step 02:call getplants operation a second time
The second time you call `PlantsController.GetPlantsAsync` you'll get a __cache hit__ and latency will be __5ms__.

![alt text](<docs/003.01 - cache miss log.png>)

# Reference 
The following articles discuss the details of `Diginsight.SmartCache` use and configuration:

- [Diginsight SmartCache](https://github.com/diginsight/smartcache) repository<br>
includes diginsight smartcache source __code__ and __documentation__

- [HowTo: Cache data, Invalidate entries and reload cache on invalidation](<docs/articles/01. Cache data, Invalidate entries and reload cache on invalidation/Cache data, Invalidate entries and reload cache on invalidation.md>)<br>
discusses how to cache calls, and add support for invalidation and reload to cached data. 

- [HowTo: Synchronize cache entries across application instances with ServiceBusCompanion or KubernetesCompanion](<docs/articles/02. Synchronize cache entries across application instances/Synchronize cache entries across application instances.md>).<br>
discusses how to configure the ServiceBusCompanion or the KubernetesCompanion to support distributed cache entries across application instances. 

- [HowTo: Configure SmartCache size, latencies, expiration, instances synchronization and RedIs integration](<docs/articles/03. Configure SmartCache size, latencies, expiration, instances synchronization and RedIs integration/Configure SmartCache size, latencies, expiration, instances synchronization and RedIs integration.md>).<br>
discusses how to configure cache size, expiration latencies and connection to external RedIs backing storage. 

- [HowTo: Boost application performance with age sensitive data management](<docs/articles/10. Boost application performance with age sensitive data management/Boost application performance with age sensitive data management.md>).<br>
discusses how performance of our applications can be boosted by using smartcache. 

- [HowTo: Enable data preloading by means of AI assisted algorithms.md]<br> 
(TODO): explores how to enable AI assisted preloading to improve data preloading efficiency.<br><br>

<!-- - [HowTo:  Boost application performance with age sensitive data management](<docs/articles/10. Leverage age sensitive data management to boost application performance.md>):<br>explores how to use `Diginsight.SmartCache` to boost application performance by means age conscious data magagement.<br>
 -->


# Additional information

Additional information is available in the following articles:<br>

>- [Introduction to diginsight smartcache](https://github.com/diginsight/smartcache?tab=readme-ov-file#introduction)<br>
>Explores __basic concepts about diginsight smartcache__ and how to integrate it<br>
>- [HowTo: Debug samples using diginsight project references](docs/Articles/01_HowTo_Debug_samples_using_diginsight_project_references/01_HowTo_Debug_samples_using_diginsight_project_references.md)<br>
>Explores how to use the samples solution and __debug__ diginsight code<br>



