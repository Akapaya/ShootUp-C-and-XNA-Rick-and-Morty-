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
    class clsExplosao
    {
        #region variaveis
        Texture2D imgExplosao;
        Vector2 Posicao;
        Rectangle retangulo;
        int largura;
        int altura;
        int tempoImagem;
        int indiceAnimacao;

        public bool ativo;
        #endregion

        #region Instanciamento
        public clsExplosao(Game jogo, ContentManager conteudo, Vector2 posicaoInicial)
        {
            imgExplosao = conteudo.Load<Texture2D>(@"images/explosao2");
            Posicao = posicaoInicial;
            ativo = true;
            largura = 129;
            altura = 115;
            retangulo = new Rectangle(0, 0, largura, altura);
        }
        #endregion

        #region Atualiza
        public void AtualizaExplosao(GameTime tempoJogo)
        {
            int tempo = tempoJogo.ElapsedGameTime.Milliseconds;
            tempoImagem += tempo;

            if (tempoImagem > 70)
            {
                indiceAnimacao++;
                if (indiceAnimacao == 10)
                    ativo = false;
                retangulo.X = largura * indiceAnimacao;
                tempoImagem = 0;
            }
        }
        #endregion

        #region Desenha
        public void DesenhaExplosao(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(imgExplosao, Posicao, retangulo, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
        #endregion
    }
}
