LeftJoinRavenDB
===============

Experiment with left join concept in C# Linq and RavenDB
A left join is not directly supported in Linq but can be done that boils down to a selectmany method. 
This linq method is not supported directly in the No-SQL database RavenDB. You have to make a sort of index view 
(term from SQL Server)  in to get support for these Linq Queries. It is made with a map reduce index.

This project tries to show how a left join in linq is done and how to implement it in RavenDB.
It is made so it can function as a workbench for experiments of this sort.
