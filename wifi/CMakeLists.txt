cmake_minimum_required(VERSION 3.17)
cmake_policy(SET CMP0091 NEW)
project(swifi VERSION 0.1.0 LANGUAGES C DESCRIPTION "Wifi for windows")
set(CMAKE_CXX_STANDARD 17)

set(ENABLE_NATIVE_WIFI ON)

file(GLOB SWIFI_SRCS
	swifi.c
)

if(ENABLE_NATIVE_WIFI)
	add_definitions(/D__SWIFI_ENABLE_NATIVE_WIFI__)
	file(GLOB SWIFI_NATIVE_WIFI_SRCS
		private/nativewifi.c
	)
	list(APPEND SWIFI_SRCS ${SWIFI_NATIVE_WIFI_SRCS})
	if(WIN32)
		list(APPEND SWIFI_LIBS Wlanapi)
	endif()
endif()

add_library(swifi ${SWIFI_SRCS})
# if(NOT "${SWIFI_LIBS}" EQUAL "")
target_link_libraries(swifi PUBLIC ${SWIFI_LIBS})
# endif()

target_include_directories(swifi PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})

add_executable(swifi_test swifi_test.c)
target_link_libraries(swifi_test PRIVATE swifi)

#enable_test()
#add_test(swifi_test)
