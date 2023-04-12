using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RickAndMorty
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Texto
        SpriteFont fonte;
        #endregion

        #region Etapas
        Texture2D telaInicial;
        Texture2D telaFimVencedor;
        Texture2D telaFimPerdedor;
        clsEtapas etapa = clsEtapas.TelaInicio;
        #endregion

        #region fundo
        Texture2D background;
        clsFundoAnim background2;
        clsFundoAnim background3;
        #endregion

        #region player
        clsPlayer player;
        int pontos = 0;
        #endregion

        #region Mina
        List<clsMina> minas;
        TimeSpan tempoInstanciamento;
        TimeSpan tempoInstanciamentoAnterior;
        Random random;
        #endregion

        #region Phaser
        List<clsPhasers> phasers;
        bool atirar = false;
        #endregion

        #region explosao
        List<clsExplosao> explosoes;
        #endregion

        SoundEffect somPhaser;
        SoundEffect somExplosao;
        Song Musica;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            #region Tela
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            IsMouseVisible = false;
            Window.Title = "Rick and Morty";
            graphics.ApplyChanges();
            #endregion

            #region FundoAnimado
            background2 = new clsFundoAnim(Content, @"images\fundo3", graphics, -3);
            background3 = new clsFundoAnim(Content, @"images\fundo2", graphics, -1);
            #endregion

            #region Player
            player = new clsPlayer(this, Content, new Vector2(10, 500));
            #endregion

            #region Mina
            minas = new List<clsMina>();
            tempoInstanciamentoAnterior = TimeSpan.Zero;
            tempoInstanciamento = TimeSpan.FromSeconds(1.0f);
            random = new Random();
            #endregion

            #region phaser
            phasers = new List<clsPhasers>();
            #endregion

            #region explosoes
            explosoes = new List<clsExplosao>();
            #endregion

            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Fonte
            fonte = Content.Load<SpriteFont>(@"fonts/fonteJogo");
            #endregion

            #region Etapas
            telaInicial = Content.Load<Texture2D>(@"images\PRINCIPAL");
            telaFimPerdedor = Content.Load<Texture2D>(@"images\derrota");
            telaFimVencedor = Content.Load<Texture2D>(@"images\vitoria");
            #endregion

            #region fundo
            background = Content.Load<Texture2D>(@"images\fundo1");
            #endregion

            Musica = Content.Load<Song>(@"sounds\music");
            somExplosao = Content.Load<SoundEffect>(@"sounds\explosao");
            somPhaser = Content.Load<SoundEffect>(@"sounds\laser");
            TocaMusica(Musica);

        }


        protected override void UnloadContent()
        {

        }

        #region Sons
        public void TocaMusica(Song musica)
        {
            MediaPlayer.Play(musica);
            MediaPlayer.IsRepeating = true;
        } 
        #endregion

        public void AdicionaMina()
        {
            int posicaoYrandomica = random.Next(100, GraphicsDevice.Viewport.Height - 100);
            clsMina mina = new clsMina(this, Content, new Vector2(1050, posicaoYrandomica));
            minas.Add(mina);
        }

        public void AtualizarMinas(GameTime tempoJogo)
        {
            #region Verifica Nova Mina
            if (tempoJogo.TotalGameTime - tempoInstanciamentoAnterior > tempoInstanciamento)
            {
                tempoInstanciamentoAnterior = tempoJogo.TotalGameTime;
                AdicionaMina();
            }
            #endregion

            #region Atualizar Mina
            for (int i = 0; i < minas.Count; i++)
            {
                minas[i].AtualizaMina(tempoJogo);
                if (minas[i].Vida <= 0)
                {
                    AdicionarExplosao(minas[i].Posicao);
                    somExplosao.Play();
                    minas[i].ativo = false;
                    minas.RemoveAt(i);
                    pontos += minas[i].valorPontos;
                    if (pontos == 400)
                    {
                        TocaMusica(Musica);
                        etapa = clsEtapas.FimJogoVencedor;
                        pontos = 0;
                        player.Vida = 100;
                        
                    }
                }
            }
            #endregion

        }

        public void AdicionarPhaser(Vector2 posicaoPhaser)
        {
            clsPhasers phaser = new clsPhasers(this, Content, posicaoPhaser);
            phasers.Add(phaser);
        }

        public void VerificaColisoes()
        {
            Rectangle objeto1;
            Rectangle objeto2;
            #region enterprise - minas
            objeto1 = player.caixaColisao();
            for (int i = 0; i < minas.Count; i++)
            {
                objeto2 = minas[i].caixaColisao();
                if (objeto1.Intersects(objeto2))
                {
                    player.Vida -= minas[i].Dano;
                    minas[i].Vida = 0;
                    if (player.Vida <= 0)
                    {
                        player.ativo = false;
                        TocaMusica(Musica);
                        etapa = clsEtapas.FimJogoPerdedor;
                        pontos = 0;
                        player.Vida = 100;
                    }
                }
            }
            #endregion
            #region Phaser - Mina
            for (int i = 0; i < phasers.Count; i++)
            {
                for (int j = 0; j < minas.Count; j++)
                {
                    objeto1 = phasers[i].caixaColisao();
                    objeto2 = minas[j].caixaColisao();
                    if (objeto1.Intersects(objeto2))
                    {
                        minas[j].Vida -= phasers[i].Dano;
                        phasers[i].ativo = false;
                    }
                }

            }

            #endregion
        }

        public void AdicionarExplosao(Vector2 posicaoExplosao)
        {
            clsExplosao explosao = new clsExplosao(this, Content, new Vector2(posicaoExplosao.X, posicaoExplosao.Y - 30f));
            explosoes.Add(explosao);
        }

        public void AtualizaExplosoes(GameTime tempoJogo)
        {
            for (int i = 0; i < explosoes.Count; i++)
            {
                explosoes[i].AtualizaExplosao(tempoJogo);
                if (!explosoes[i].ativo)
                    explosoes.RemoveAt(i);
            }
        }


        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            switch (etapa)
            {
                case clsEtapas.TelaInicio:
                    #region Inicio
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        etapa = clsEtapas.Jogo;
                    }
                    #endregion
                    break;
                case clsEtapas.Jogo:
                    MediaPlayer.Stop();
                    #region Etapas
                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        etapa = clsEtapas.FimJogoPerdedor;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.V))
                    {
                        etapa = clsEtapas.FimJogoVencedor;
                    }
                    #endregion

                    #region Atualiza Fundo
                    background2.AtualizaFundo();
                    background3.AtualizaFundo();
                    #endregion

                    #region Mina
                    AtualizarMinas(gameTime);
                    #endregion

                    #region Player
                    player.AtualizaPlayer(gameTime, graphics);
                    #endregion

                    #region Phaser
                    if (Keyboard.GetState().IsKeyDown(Keys.F))
                    {
                        if (atirar)
                        {

                            AdicionarPhaser(player.Posicao + new Vector2(player.largura - 30f, player.altura / 2 - 10f));
                            somPhaser.Play();
                            atirar = false;
                        }
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.F))
                    {
                        atirar = true;
                    }
                    for (int i = 0; i < phasers.Count; i++)
                    {
                        phasers[i].AtualizaPhaser(gameTime, graphics);
                        if (phasers[i].ativo == false)
                        {
                            phasers.RemoveAt(i);
                        }
                    }
                    #endregion

                    #region Colisoes

                    VerificaColisoes();

                    #endregion

                    AtualizaExplosoes(gameTime);

                    break;
                case clsEtapas.FimJogoVencedor:
                    #region Vencedor
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        etapa = clsEtapas.TelaInicio;
                    }
                    #endregion
                    break;
                case clsEtapas.FimJogoPerdedor:
                    #region Perdedor
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        etapa = clsEtapas.TelaInicio;
                    }
                    #endregion
                    break;

            }



            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (etapa)
            {
                case clsEtapas.TelaInicio:
                    spriteBatch.Draw(telaInicial, Vector2.Zero, Color.White);
                    break;
                case clsEtapas.Jogo:

                    #region jogo

                    #region fundo
                    spriteBatch.Draw(background, Vector2.Zero, Color.White);
                    #endregion



                    #region DesenhaFundo
                    background2.DesenharFundoAnim(spriteBatch);
                    background3.DesenharFundoAnim(spriteBatch);
                    #endregion

                    #region Player
                    player.DesenhaPlayer(spriteBatch);
                    #endregion

                    #region Phaser
                    for (int i = 0; i < phasers.Count; i++)
                    {
                        phasers[i].DesenhaPhaser(spriteBatch);
                    }
                    #endregion

                    #region Mina
                    for (int i = 0; i < minas.Count; i++)
                    {
                        minas[i].DesenhaMina(spriteBatch);
                    }
                    #endregion

                    for (int i = 0; i < explosoes.Count; i++)
                    {
                        explosoes[i].DesenhaExplosao(spriteBatch);
                    }

                    spriteBatch.DrawString(fonte, "pontos: " + pontos, new Vector2(10, 0), Color.White);
                    spriteBatch.DrawString(fonte, "vida: " + player.Vida, new Vector2(10, 30), Color.White);

                    #endregion

                    break;
                case clsEtapas.FimJogoVencedor:
                    spriteBatch.Draw(telaFimVencedor, Vector2.Zero, Color.White);
                    break;
                case clsEtapas.FimJogoPerdedor:
                    spriteBatch.Draw(telaFimPerdedor, Vector2.Zero, Color.White);
                    break;

            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
