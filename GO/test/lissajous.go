package test

import (
	"image"
	"image/color"
	"image/gif"
	"io"
	// "io"
	"math"
	"math/rand"
	"os"
)

var palettle = []color.Color{color.White, color.Black, color.RGBA{0xff, 0x0, 0, 0xff}, color.RGBA{0, 0xff, 0, 0xff},
	color.RGBA{0, 0x0, 0xff, 0xff}}

const (
	whiteIndex = 0
	blackIndex = 1
)

func Lissajous(out io.Writer, cycles float64) {
	// out := os.Stdout
	if out == nil {
		out = os.Stdout
	}
	if cycles <= 0 {
		cycles = 5
	}
	const (
		res     = 0.001
		size    = 100
		nframes = 64
		delay   = 8
	)

	freq := rand.Float64() * 3.0
	anim := gif.GIF{LoopCount: nframes}
	phase := 0.0
	for i := 0; i <= nframes; i++ {
		rect := image.Rect(0, 0, 2*size+1, 2*size+1)
		img := image.NewPaletted(rect, palettle)
		for t := 0.0; t < cycles*2*math.Pi; t += res {
			x := math.Sin(t)
			y := math.Sin(t*freq + phase)
			img.SetColorIndex(size+int(x*size+0.5), size+int(y*size+0.5), 4)
		}
		phase += 0.1
		anim.Delay = append(anim.Delay, delay)
		anim.Image = append(anim.Image, img)
	}
	gif.EncodeAll(out, &anim)
}
