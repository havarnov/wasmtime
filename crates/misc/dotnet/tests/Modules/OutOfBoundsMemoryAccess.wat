(module
  (import "" "mem" (memory $m 1 2))
  (data (i32.const 65535) "\01")
  (data (i32.const 65536) "\02")
)
