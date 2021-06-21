Some future changes / what I didn't complete:

Extract values such as parcel size price and criteria for parcel type classifications to a configuration object of sorts to allow the processor to be configured with the values instead of hardcoding them)
Also for the weight limit values associated with specific parcel types.

Improve exception throwing and add concrete exception types instead of using the base system one

The only public method for the API should be CalculateOrder, all the other methods are internally used but left public for ease of unit testing. There are ways to get around this by making them private and then testing it using the PrivateType wrapper.

The constructor arguments for Parcel might benefit from a builder where you explicitly set the various dimensions to avoid potentially passing in arguments in the wrong order.

Didn't get around to fully fleshing out the unit testing for the application of discounts to an order
