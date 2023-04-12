using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RickAndMorty
{
    class clsFundoAnim
    {
        #region variaveis
        Texture2D imagem;
        Vector2[] posicoes;
        int velocidade;
        #endregion

        #region Instanciamento
        public clsFundoAnim(ContentManager conteudo, string nomeArquivo, GraphicsDeviceManager tela, int velocidadeInicial)
        {
            imagem = conteudo.Load<Texture2D>(nomeArquivo);
            velocidade = velocidadeInicial;
            posicoes = new Vector2[tela.GraphicsDevice.Viewport.Width / imagem.Width + 1];

            for (int i = 0; i < posicoes.Length; i++)
            {
                posicoes[i] = new Vector2(i * imagem.Width, 0);
            }
        }
        #endregion


        #region Atualiza
        public void AtualizaFundo()
        {
            if (velocidade > 0)
                velocidade *= -1;
            for (int i = 0; i < posicoes.Length; i++)
            {
                posicoes[i].X += velocidade;
                if (posicoes[i].X <= -imagem.Width)
                {
                    posicoes[i].X = imagem.Width;
                }
            }
        }

        #endregion

        #region Desenhar
        public void DesenharFundoAnim(SpriteBatch spritebatch)
        {
            for (int i = 0; i < posicoes.Length; i++)
            {


                spritebatch.Draw(imagem, posicoes[i], Color.White);
            }
        }
        #endregion

    }
}
