.segment "CODE"

.org $0300

entry:
  lda #$01
top:
  sta $8000
  asl
  bcs entry
  bcc top