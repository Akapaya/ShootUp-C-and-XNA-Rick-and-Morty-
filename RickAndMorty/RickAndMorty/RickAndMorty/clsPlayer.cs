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
    class clsPlayer
    {
        #region Variaveis
        Texture2D imgPlayerParado;
        Texture2D imgPlayerAnim;
        Texture2D imgPlayerAtual;

        public Vector2 Posicao;
        float velocidade;

        Rectangle retangulo;
        public int largura;
        public int altura;
        int tempoImagem;
        int indiceAnimacao;

        public bool ativo;
        public int Vida;
        #endregion

        #region Instanciamento
        public clsPlayer(Game jogo, ContentManager conteudo, Vector2 posicalInicial)
        {
            Posicao = posicalInicial;
            velocidade = 0.5f;
            imgPlayerParado = conteudo.Load<Texture2D>(@"images\nave");
            imgPlayerAnim = conteudo.Load<Texture2D>(@"images\naveanim");
            imgPlayerAtual = imgPlayerParado;
            largura = imgPlayerAtual.Width;
            altura = imgPlayerAtual.Height;
            retangulo = new Rectangle(0, 0, largura, altura);
            indiceAnimacao = 0;
            ativo = true;
            Vida = 100;
        }
        #endregion

        #region Atualiza o Player
        public void AtualizaPlayer(GameTime tempoJogo, GraphicsDeviceManager tela)
        {
            #region Tempo
            int tempo = tempoJogo.ElapsedGameTime.Milliseconds;

            tempoImagem += tempo;
            #endregion

            #region Muda indice e posicao
            if (tempoImagem > 100)
            {
                indiceAnimacao++;
                if (indiceAnimacao == 2)
                    indiceAnimacao = 0;
                retangulo.X = largura * indiceAnimacao;
                tempoImagem = 0;
            }
            #endregion

            #region Controle

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Down)
                || Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {

                imgPlayerAtual = imgPlayerAnim;

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Posicao.X += tempo * velocidade;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Posicao.X -= tempo * velocidade;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    Posicao.Y -= tempo * velocidade;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    Posicao.Y += tempo * velocidade;
                }
            }
            else
            {
              
                imgPlayerAtual = imgPlayerAnim;
                //imgPlayerAtual = imgPlayerParado;
                // retangulo.X = 0;
                //indiceAnimacao = 0;
            }
            #endregion

            #region Limite tela
            Posicao.X = MathHelper.Clamp(Posicao.X, 0, tela.GraphicsDevice.Viewport.Width - largura);
            Posicao.Y = MathHelper.Clamp(Posicao.Y, 0, tela.GraphicsDevice.Viewport.Height - altura);
            #endregion
        }

        #endregion

        #region Desenha o Player
        public void DesenhaPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(imgPlayerAtual, Posicao, retangulo, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
        #endregion

        #region Caixa de Colisao
        public Rectangle caixaColisao()
        {
            Rectangle temporario = new Rectangle((int)Posicao.X, (int)Posicao.Y, largura, altura);
            return temporario;
        }
        #endregion
    }
}
