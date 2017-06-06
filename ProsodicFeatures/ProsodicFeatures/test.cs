using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProsodicFeatures
{
    class test
    {
        private static string out_put_text;

        static void Main(string[] args)
        {

            //ProsodyFeaturesII p = new ProsodyFeaturesII(@"<Speech_joy><ANIMATE(joy1)><Gaze(person3)>O pequeno país onde tu vives tem uma vegetação exuberante, o teu povo vive feliz e canta pelas ruas.  
            //  </Speech_joy><Speech_anger>Contudo, sem nenhum aviso o teu país recebe uma <ANIMATE(Anger1)> ameaça de invasão de um país vizinho. <Gaze(Person2)>Como seu representante, <Gaze(person3)> 
            //   deverás tomar diversas decisões e as tuas escolhas irão determinar se o teu povo irá ou não cair nas mãos do inimigo. 
            //</Speech_anger>Desde já, deves começar a agir e escolher uma das seguintes decisões:");

         //    ProsodyFeaturesII p = new ProsodyFeaturesII(@"<speech_joy><Gaze(person3)>Com o baile, consegues fazer com que parte da <ANIMATE(joy1)>população se sinta tranquila, </speech_joy><speech_sadness>porém, outra parte acha que o baile, é apenas para <ANIMATE(sadness2)> esconder a gravidade da situação.</speech_sadness> Então,<Gaze(person3)> vais para a tua residência e mandas começar os preparativos para o baile, avisas que será esta noite. A preparação é rápida, pois a qualquer momento pode-se iniciar a batalha. <Gaze(person2)><speech_joy>Chega à noite, a banda começa a tocar e <ANIMATE(joy3)>todos divertem-se. </speech_joy><Gaze(person3)>Tu apenas observas, notas que o povo está se sentindo mais confiante para a batalha. Em uma situação como esta, tu:");

               ProsodyFeaturesII p = new ProsodyFeaturesII(@"<Gaze(person3)>Os mensageiros avisam todos para se reúnirem na praça central. Indo para lá, <Gaze(person2)>percebes que há muitos murmúrios pelas ruas! <ANIMATE(Fear2)><Gaze(person3)><Speech_Anger>Ao chegar ao palco, respiras fundo, e começas a subir as escadas que dão acesso ao local de discurso, notas um silêncio respeitoso do povo que anseia por explicações e orientações.</Speech_Anger> <Gaze(person2)>Sentes que deves fazer uma declaração que acalme a população, porque a espectativa de uma batalha está gerando o princípio do caos. <Gaze(person3)>Deste modo, declaras que: ");


            p.InsertProsodyFeatures();
            out_put_text = p.OutPut_animationText;

            Console.WriteLine("out_put_text = " + out_put_text);

        }

    }
}
