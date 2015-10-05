# Micro Edge Case
A project intended to build a cardboard desktop version of the [#microsoftedgecase](https://twitter.com/hashtag/microsoftedgecase) on [Windows 10 UAP](http://ms-iot.github.io/content/en-US/win10/SetupRPI.htm)


## Edge Case App

At the moment it's a basic app that takes in a URL, calls the API and saves all of the JSON items returned using MemoryStreams and DataContracts (see the EdgeCaseModel.cs). The results currently displayed in the UI are:

- Browser Detection: pass/fail
- Markup: pass/fail
- Plugin Free: pass/fail
- JS Libs: pass/fail
- Edge: pass/fail
- CSS Prefixes: pass/fail

### TO-DO:

- UI
- Organise which results will be displayed and how
- Discuss how RPi/IoT version will work?
