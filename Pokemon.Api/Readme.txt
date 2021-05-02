#Set up Pokemon.Api as the start up project

# The Pokemon Api has two end points 


#1. Get Data

#Sample requests
https://localhost:5001/pokemon/metwo - Valid data returned


https://localhost:5001/pokemon/cbfgh - Error response returned

#2.Get Translated data for description passed in

#Sample requests

https://localhost:5001/pokemon/translated/metwo - valid data returned

https://localhost:5001/pokemon/translated/ - Error response returned

#The external Api's are public Api's and have a rate limit of 5 request per hour

