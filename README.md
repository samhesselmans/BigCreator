# BigCreator
A simple class to create EA .BIG files

This is a simple c# class that can create EA .BIG Files.

I needed this for one of my own projects, but there was nothing to use, so I wrote my own. 

Right now it creates a BIG file from a folder path, where it takes all the files in this folder and it's subdirectories and saves it to the given output file.

I also added the dontIncludeParentDir option which does the folowwing:
  Let's say you have a folder called "mod" in which you have to sub folders: "data" and "maps" in which there are some files
  Now with dontIncludeParentDir on false we get the following file names in our big file:
          "mod\data\<YourFileNames>"
          "mod\maps\<YourFileNames>"
  With it on true we get:
          "data\<YourFileNames>"
          "maps\<YourFileNames>"
          


I also included a small console app, which you could simply use by calling it as follows:
  "CreateBig.exe <YourModFolder> <YourBigDestinationFolder> [-sr]"
  Where -sr is optional. The -sr (Skip root) is for the dontIncludeParentDir option
