## MVC Sort Test
A MVC ASP.NETcore app that can be used in conjunction with a MYSQL database to create and sort comma spaerated intergers.

## Public endpoints
- /Home
- /Home/About
- /SortEntries
- /SortEntries/Create
- /SortEntries/Delete/#
- /SortEntries/Details/#

## Testing Installation
- Requires VisualStudio 2022 .NET 6.0
- Clone or fork this repo or download the zipfile.
- Open the MVC_Sot_Test.sln with visual studio(2022).
- (Create a SQL Local Db for testing) 
- Tools -> NuGet Package Manager -> Package Manager Console: 
- Run command  "Update-Database"
```
PM> Update-Database
```

<h1>About this website.</h1>
<h3>Summary</h3>
<p>
	This website is a simple integer sorter built with MVC ASP.NET core. Its goal is to sort integers that are provided in a CSV format,
	the user can decide to sort ascending or descending using radio toggles in the create page.
	The user can also download a json file, view details, edit or delete entries.
</p>
<p>
	To make the app more visual I have used Chart.js to draw an interactable graph of Sort Time against Integer Count.
	This enables easier understanding of the presented data, and allows clicking on a plot point to show its details.
	This makes it easier to find, see details of and delete specific entries.
</p>
<p>
	I decided to use comma separated values as the input as this seemed the simplest way for a user to manually enter a list of numbers.
	In addition, this approach allows a file with comma separated values to be uploaded and sorted (I have not yet implemented file upload).
</p>
<h3>Issues</h3>
<h5>Sorting speed inconsistency:</h5>
<p>
	When sorting a user inputted CSV the sort operation time seems to fluctuate. I dont not fully understand why this is but i assume it is to do with the underlying code
	behind the array.sort() method. I created a way to randomly generate 1000 CSV entries of variable length.
	These are sorted just like a user entered CSV. With 1000 entries the graph shows that mostly the sort times fit to a neat line but a few entries will not.
	The first entry is always slow. This problem means that the Sort Time for every user generated CSV will vary greatly,
	the sort time is technically correct as that is how long it took to sort the array, but it may not fit the expected gradient.
</p>
<h6>Possible fixes:</h6>
<p>
	<ul>
		<li>Run each sort many times and take an average.</li>
		<li>Run sort once and compare to a pre computed gradient of expected time results. Run again if SortTime is not within tolerance.</li>
	</ul>
</p>
<h5>High memory usage:</h5>
<p>
	Using the memory profiler in visual studio you can see that the memory use rises as you add entries.
	Adding a few thousand entries using the random buttons will bring the memory usage up above 1gb and beyond.
	I am not sure why this is happening. I changed a few lines in the SortEntry.sort() that seemed to bring down
	the memory usage a lot.
<pre>
<code>
	string[] split = OriginalCSV.Split(',');
	foreach (var s in split) { ... }

	Changed to:

	foreach (var s in OriginalCSV.Split(',')) { ... }	
</code>
</pre>
	But from reading it seems that the garbage collection is complicated. It is possible memory usage is as expected
	in VS debug mode on my dev system that has plenty of RAM.
</p>

<h3>Feature ideas</h3>
<h5>Sorting Algorithm Choices:</h5>
<p>
	It would be interesting to see the speed difference between different sort algorithms represented on the graph.
	I chose to use Array.Sort as it seems to be the fastest option.
	Implementing this would require much more knowledge than I have on sorting algorithms and the underlying code
	to Array.Sort.
</p>
<h5>File sort:</h5>
<p>
	To be able to upload a file of CSVs to be sorted. I believe this would be easy to implement.
</p>
<h3>Conclusion</h3>
<p>
	I have enjoyed building this project, mostly I have enjoyed visualising the data and have gained a better understanding of the MVC pattern. I made a few mistakes in the beginning, one mistake was to put all the sorting logic in the controller. After reading a little I realised that the logic should probably be in the model not the controller.
	I am still unsure whether using a CSV as the input method is the right way to go. It seems like the most sensible way for a user to input any numbers they would like. CSV is a common file format that is widely used enabling easy setup of a file upload with lists of CSV to be sorted.

</p>
<p>
	Although not complete if feel this app is in a good working state. I have many ideas for improvements and will likely continue to work on it. It has been a valuable and rewarding learning experience for me and I feel I have learnt a lot about MVC and web development.
</p>
<p>
	Hayden Shore.
</p>
