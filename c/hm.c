#include<math.h>
#include<stdio.h>
#include<stdint.h>
#include<string.h>

int main (void)
{
double hertz[] = { 523.25, 587.33, 523.25, 0, 523.25, 587.33, 523.25, 0 };
int num_notes = sizeof(hertz)/sizeof(hertz[0]);
int samples_per_note = 5512;
int samples_per_rest = (44100/2)-5512;
int total_secs = 4;
int anti_pop_samples = 0;
double max_amplitude = 20000.0;

typedef union { uint16_t dword; uint8_t bytes[2]; } WORD;
typedef union { uint32_t dword; uint8_t bytes[4]; } DWORD;

struct {
    char riff[4];
    DWORD file_size;
    char wave[4];
    char fmt[4];
    DWORD wave_chunk_size;
    WORD wave_type;
    WORD num_channels;
    DWORD num_samples_second;
    DWORD num_bytes_second;
    WORD block_alignment;
    WORD num_bits_sample;
    char data[4];
    DWORD data_size;
} __attribute__ ((packed)) header = 
    { "RIFF", 44100*1*2*total_secs+sizeof(header)-8, "WAVE", "fmt ", 16, 1, 1, 44100, 44100*1*2, 1*16/8, 16, "data", 44100*1*2*total_secs };

fwrite(&header, sizeof(header), 1, stdout);

for (int i = 0; i < num_notes; i++)
{
    for (int j = 0; j < samples_per_note; j++)
    {
        int16_t sample;
    
        double pop_removal = j >= anti_pop_samples ? 1.0 : (double)j / anti_pop_samples;
        double magnitude = (max_amplitude * (samples_per_note - j)) / samples_per_note;
        double sine;
        sine = sin(hertz[i] * j / 44100.0 * 2.0 * M_PI);
        sample = (int16_t)(pop_removal * magnitude * sine);
        fwrite(&sample, sizeof(sample), 1, stdout);
    }
    for (int j = 0; j < samples_per_rest; j++)
    {
        int16_t sample = 0;
        fwrite(&sample, sizeof(sample), 1, stdout);
    }
}

return 0;
}
