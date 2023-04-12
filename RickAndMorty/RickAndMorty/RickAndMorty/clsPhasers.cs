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
    class clsPhasers
    {
        #region variaveis
        Texture2D Imagem;
        Vector2 Posicao;
        public bool ativo;
        float velocidade;
        int largura;
        int altura;
        public int Dano;
        #endregion

        #region Instanciamento
        public clsPhasers(Game jogo, ContentManager conteudo, Vector2 posicaoInicial)
        {
            Imagem = conteudo.Load<Texture2D>(@"images\phaser");
            Posicao = posicaoInicial;
            ativo = true;
            velocidade = 20f;
            largura = Imagem.Width;
            altura = Imagem.Height;
            Dano = 20;
        }
        #endregion

        #region Atualiza
        public void AtualizaPhaser(GameTime tempoJogo, GraphicsDeviceManager tela)
        {
            Posicao.X += velocidade;
            if (Posicao.X > tela.GraphicsDevice.Viewport.Width)
            {
                ativo = false;
            }
        }
        #endregion

        #region Desenha
        public void DesenhaPhaser(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Imagem, Posicao, Color.White);
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
