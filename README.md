# -Prototyping_004_AgnieszkaZielke_SignLanguageLearning
A VR App for learning sign languages

## Idea

My grandfather has recently completely lost hearing in both ears, after about 15 years of steady decline caused by Ménière's disease. During that time neither himself or my family made meaningful effort to learn sign language, as it was considered ‘too complicated’ and ‘too much effort’. As a result, the only way to communicate with my grandfather now is via handwritten notes, slowing down communication considerably and making him feel isolated and overlooked. 

I believe this is a common trajectory for a lot of families which could be very different if only sign language learning was made more accessible and casual, similar to to playing a fun game. The accurate hand tracking feature of the Oculus Integration SDK offers a new exciting way of bringing sign gesture learning into VR:

- Instead of trying to replicate sign gestures from 2D pictures / videos, the user can accurately mimic the right position in 3D with feedback from the app
- Due to the immersion in the virtual world, the user maintains focus
- The learning is made fun and engaging due to the gamification of the process.

 

## Features

**Core features:** 

- Static sign detection (i.e. no movement, pose detection)
- Detect hand poses and show the user how close he/ she it to getting the pose right (via the hand skeleton debugging)
- Create a short inventory of sign positions - this may be done via the hand pose recorder
- Guide the user to follow along with the hand poses

**Stretch goals:** 

These are features that are nice to have, and that you might implement if you have extra time, or in the future.

(I think I’m unlikely to implement any of these this week, but would be excited to work on them after the bootcamp)

- Dynamic sign detection (i.e. gestures)
- Different sign language ‘games’ to help the user practice
- Option to learn different signing languages (e.g. BSL & ASL)
- Store signs already learnt for future repetition (via PlayerPrefs)

## Tech Stack

- Unity 2021.3.11f1
- VR prototype
- Oculus Integration SDK; hand tracking only

## Resources/Inspiration

[https://sidequestvr.com/app/1317/asl-fingerspeller](https://sidequestvr.com/app/1317/asl-fingerspeller)

[https://learnvr.org/learn-sign-language-with-virtual-reality-and-hand-tracking/](https://learnvr.org/learn-sign-language-with-virtual-reality-and-hand-tracking/)
