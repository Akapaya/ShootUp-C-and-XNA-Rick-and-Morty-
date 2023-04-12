using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RickAndMorty
{
    class clsMina
    {
        #region variaveis
        Texture2D imgMinaParada;
        Texture2D imgMinaAnim;
        Texture2D imgMinaAtual;

        public int valorPontos;
        public Vector2 Posicao;
        float velocidade;
        Rectangle retangulo;
        int largura;
        int altura;
        int tempoImagem;
        int indiceAnimacao;

        public bool ativo;
        public int Vida;
        public int Dano;
        #endregion

        public clsMina(Game jogo, ContentManager conteudo, Vector2 posicaoInicial)
        {
            Posicao = posicaoInicial;
            velocidade = 0.4f;
            imgMinaParada = conteudo.Load<Texture2D>(@"images\inimigo");
            imgMinaAnim = conteudo.Load<Texture2D>(@"images\inimigo_anim");
            imgMinaAtual = imgMinaParada;
            largura = imgMinaAtual.Width;
            altura = imgMinaAtual.Height;
            retangulo = new Rectangle(0, 0, largura, altura);
            indiceAnimacao = 0;
            imgMinaAtual = imgMinaAnim;
            ativo = true;
            Vida = 100;
            Dano = 10;
            valorPontos = 20;
        }

        public void AtualizaMina(GameTime tempoJogo)
        {

            int tempo = tempoJogo.ElapsedGameTime.Milliseconds;

            tempoImagem += tempo;

            if (tempoImagem > 100)
            {
                indiceAnimacao++;
                if (indiceAnimacao == 2)
                    indiceAnimacao = 0;
                retangulo.X = largura * indiceAnimacao;
                tempoImagem = 0;
            }
            Posicao.X -= tempo * velocidade;

            if (Posicao.X + largura < 0 || Vida <= 0)
            {
                ativo = false;
            }
        }

        public void DesenhaMina(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(imgMinaAtual, Posicao, retangulo, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        #region Caixa de Colisao
        public Rectangle caixaColisao()
        {
            Rectangle temporario = new Rectangle((int)Posicao.X, (int)Posicao.Y, largura, altura);
            return temporario;
        }
        #endregion
    }
}
