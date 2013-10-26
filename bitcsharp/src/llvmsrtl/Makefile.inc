prefix = /i/Backup/Skydrive/Documents/GitHub/Bitcsharp/bitcsharp/src/llvmsrtl

include $(prefix)/config

ifeq ($(USE_C_STDLIB), 1)
  USE_C_STDLIB_CONFIG = true
  CCFlags += -D"USE_C_STDLIB=1"
else
  USE_C_STDLIB_CONFIG = false
  CCFlags = -nostdlib -nodefaultlibs
  CCFlags += -D"USE_C_STDLIB=0"
endif

CCFlags += -D"OSTYPE=$(OSTYPE)"
CCFlags += -D"ARCH=$(ARCH)"
