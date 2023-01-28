.segment "CODE"

.org $0300

ldx #$FF
txs

lda #$40
sta $8000

main:
  lda #'>'
  jsr put_char
  lda #' '
  jsr put_char
  ldx #$00
main_inner:
  txa
  pha
  jsr get_char
  tay
  pla
  tax
  tya
  sta $0400,X
  cmp #$0D
  beq print_buffer
  txa
  pha
  tya
  jsr put_char
  pla
  tax
  inx
  beq print_buffer_full
  bne main_inner
print_buffer_full:
  dex
print_buffer:
  txa
  pha
  lda #$0A
  jsr put_char
  lda #$0D
  jsr put_char
  ldy #$00
pbuf_top:
  tya
  pha
  lda $0400,Y
  jsr put_char
  pla
  tay
  iny
  pla
  tax
  dex
  txa
  pha
  bne pbuf_top
  pla
  jmp main

put_char:
  ldx #$00
  stx $8000
  jsr send_wait
  ldx #$08
send_next:
  pha
  and #$01
  clc
  ror
  ror
  ror
  sta $8000
  jsr send_wait
  pla
  lsr
  dex
  bne send_next

  lda #$40
  sta $8000
  jsr send_wait
  jsr send_wait
  jsr send_wait

  rts

get_char:
  lda #$00
  pha
wait_start:       
  lda $8000      
  and #$20       
  bne wait_start

  jsr rcv_wait  ;6

  ldx #$08       ;2
nextbit:
  jsr rcv_wait  ;6
  jsr rcv_wait  ;6

  lda $8000      ;4
  and #$20       ;2
  tay            ;2

  pla            ;4
  lsr            ;2
  cpy #$20       ;2
  bne nobit      ;3
  ora #$80       ;2
nobit:
  pha            ;3

  dex            ;2
  bne nextbit    ;3

  jsr rcv_wait
  jsr rcv_wait
  jsr rcv_wait
  jsr rcv_wait

  pla ;result
  rts

rcv_wait:
  ldy #47
rwait_y:
  dey            ;2
  bne rwait_y     ;3
  rts            ;6

send_wait:
  ldy #97
swait_y:
  dey
  bne swait_y
  rts

  