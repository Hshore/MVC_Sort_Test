## MVS Sort Test
A MVC ASP.NETcore app that can be used in conjunction with a MYSQL database to create and sort comma spaerated intergers.

## Public endpoints
- /Home
- /Home/About
- /SortEntries
- /SortEntries/Create
- /SortEntries/Delete/#
- /SortEntries/Details/#

## Testing Installation
- Clone or fork this repo or download the zipfile.
- Open the MVC_Sot_Test.sln with visual studio(2022).
- (Create a SQL Local Db for testing) 
- Tools -> NuGet Package Manager -> Package Manager Console: 
- Run command  "Update-Database"
```
PM> Update-Database
```

<h1>About this website.</h1>
<h2>Summary</h2>
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
<h2>Issues</h2>
<h5>Sorting speed inconsistency:</h5>
<p>
When sorting a user inputted CSV the sort operation time seems to fluctuate. I dont not fully understand why this is but i assume it is to do with the underlying code
behind the array.sort() method. I created a way to randomly generate 1000 CSV entries of variable length.
These are sorted just like a user entered CSV. With 1000 entries the graph shows that mostly the sort times fit to a neat line but a few entries will not.
The first entry is always slow. This problem means that the Sort Time for every user generated CSV will vary greatly,
the sort time is technically correct as that is how long it took to sort the array, but it may not fit the expected gradient.
</p>
<h5>Possible fixes:</h5>
<p>
    <ul>
        <li>Run each sort many times and take an average.</li>
        <li>Run sort once and compare to a pre computed gradient of expected time results. Run again if SortTime is not within tolerance.</li>
    </ul>
</p>

