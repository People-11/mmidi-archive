# mmidi-archive
Archive of Sono's mmidi files.

## What is mmidi?
Take a look at [here](https://blackmidi.fandom.com/wiki/Software:mmidi "here").

## What are those files?
Since [Sono's website](https://sono.9net.org/prog/mmidi/ "Sono's website") was down, nobody can download mmidi from there. So I got all the files and make an archive here.

## What are the differences?
### Version differences
- **mmidi**
This is the original release. It comes with a visualizer and mouse + keyboard controls.

- **mmopt**
This is just a stripped down version of mmidi. No visualizer, no controls, just text and midi playing.

- **mmm/MorshuMidi**
Written from scratch, everything stripped, very high performance. Also, quite small. Very few CPU cycles per note. The world's fastest available midi player.

- **mmmmidi**
MorshuMidi with mmidi's visualizer slapped on it. Includes a more advanced scrolling control than mmidi.

- **mmvis**
Derivation of mmmmidi. It includes some extra features at the cost of slightly degraded performance.

- **MMVis**
This is an attempt to create a Piano From Above clone for the Black MIDI community which aims to look similar to the original while outperforming it.

- **extremm / ExtremeMIDI**
This is currently the fastest and smallest MIDI player available. The original MorshuMidi was equipped with an extra processing thread, creating ExtremeMIDI. This edition also comes equipped with smart noteskip, so if note processing takes too long then, and only then will it start skipping notes.

### Other differences
- A file with **64** in the name means it is 64-bit.

- A file with **mem** in the name means it loads the midi file into RAM for playback, so midi larger than RAM cannot be played.

- A file with **map** in the name means it doesn't fully load midi into RAM for playback, so midi larger than RAM can played, but performance will be limited by Windows' file buffering and disk speed.

- A file with **d** in the name means it is a benchmark edition.

For more details, please refer to the original website.

## Original website?
The original website was down. If you still want to visit, [here](https://web.archive.org/web/20210303203008id_/https://sono.9net.org/prog/mmidi/ "here").
