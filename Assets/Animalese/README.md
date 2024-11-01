# Animalese Unity Asset

First, thank you very much for purchasing this asset! You're helping me
fund my passion for creating videogames.

This asset is designed to give your game a bit of voice without forcing you to
prepare your game's script in stone and hire voice actors who might not speak
the same language as your players.

The concept of Animalese has been used many times before but was likely made
popular by Nintendo's Animal Crossing series.

# How it works

Animalese is, essentially, playing phoenetic sounds for each character of the
English alphabet in quick succession so that it sounds at a glance like
something we can parse but really can't if we concentrate.

It works well for any language even though the base alphabet is English since
the sounds that are made are gibberish anyway!

# Usage

Any character in your game who needs a voice will require the
`Animalese` script attached. If this is your first time using Animalese, it
might be worthwhile to check out the Example section then come back when
you are finished.

## Phoenetic Sound Clips

The `Animalese` script looks for the phoenetic sound clips to use for each
of the characters in the English alphabet + the numbers zero to nine. You can
find some example voice clips in the Voices folder, but you'll probably want
to create your own and drag them onto the respective AudioClip fields on your
game object.

## Channels

Since Animalese is read very quickly, it's likely that two or more AudioClips
will play at one time. To reduce/eliminate popping or clipping of your sounds,
you can create multiple AudioSources and Animalese will loop through them in
order.

To see this in action, take a look at the example game objects in the example
scene.

Four channels generally works well unless your character needs to read
a lot of phoenetic sounds very quickly.

## Adjustable values

There are several levers you can pull to adjust how your character sounds:

* Pitch: The base pitch at which each of the phoenetic sounds will play.
Setting this higher will make your character's voice sound like a chipmunk!

* Pitch Variation: The pitch is randomly bounced between its base value and
the base value multiplied by this number as it reads your text. This gives
your character a less monotone voice.

* Multiplier Excite: Asterisks in your text demarkate "excited" speech. Any
text between asterisks will cause your character to read it at this pitch.

* Letter Delay In Seconds: This value sets how long of a period to wait while
voicing one phoenetic sound before moving onto the next one. The smaller the
number here, the faster your character will read!

* Letter Delay Variance In Seconds: Similar to Pitch Variation, this will
give a random variance in the delay between each phoenetic character read,
so that your character doesn't read in a steady pace, like a robot (unless
you want that!)

## Scripting

Any `Animalese` script attached to your character's game object can read
text by calling the `Speak` method. If you want to stop the character
halfway through a sentence, say if the character is interrupted in-game,
call the `StopSpeaking` method.

# Example

Open the scene in the Example Scene directory and look in the scene
hierarchy. There you'll find three game objects which you can enable, one at
a time, to hear the different built-in sounds.

Note that these are not good enough to use in your game as the samples are
just for demonstration purposes, but they will give you an idea of the
different kinds of voices that you can build!

# License

Since this asset lives on the Unity Asset Store, it follows the Asset Store
Terms of Use and EULA, found at this URL:

https://unity3d.com/legal/as_terms

# Contact

You can reach me at john@focusonfungames.com.
