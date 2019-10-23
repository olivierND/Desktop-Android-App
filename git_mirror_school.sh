#! /bin/sh

mkdir temp \
&& cd temp \
&& git clone https://github.com/GabrielBottari/log3900.git --mirror \
&& cd log3900.git \
&& git remote add school https://githost.gi.polymtl.ca/git/log3900-03 \
&& git push school --mirror \
&& cd ..\\.. \
&& rm -rf temp