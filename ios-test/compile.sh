#!/bin/sh

mkdir out
cd out

cmake -GXcode -DCMAKE_SYSTEM_NAME=iOS \
	 -DCMAKE_INSTALL_PREFIX=`pwd`/_install \
    -DCMAKE_XCODE_ATTRIBUTE_ONLY_ACTIVE_ARCH=NO \
    -DCMAKE_IOS_INSTALL_COMBINED=YES ..

xcodebuild

cd -

./out/main
